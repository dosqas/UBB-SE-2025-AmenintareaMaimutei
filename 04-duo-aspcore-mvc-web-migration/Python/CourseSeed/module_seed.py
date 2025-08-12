import requests
from faker import Faker
import random
import time

# Initialize Faker
fake = Faker()

# API endpoints
BASE_URL = "https://localhost:7174"
COURSE_LIST_URL = f"{BASE_URL}/api/Course/list"
MODULE_ADD_URL = f"{BASE_URL}/api/Module/add"

# Number of modules to generate per course
MODULES_PER_COURSE = 3

def get_existing_courses():
    """Fetch existing courses from the API"""
    try:
        response = requests.get(COURSE_LIST_URL, verify=False)
        response.raise_for_status()
        return response.json()
    except Exception as e:
        print(f"Error fetching courses: {str(e)}")
        return []

def generate_module(course_id):
    """Generate a fake module dictionary for a specific course"""
    return {
        "Title": fake.sentence(nb_words=4),
        "Description": fake.paragraph(nb_sentences=3),
        "IsBonus": random.choice([True, False]),
        "Cost": random.randint(10, 200),
        "ImageUrl": fake.image_url(),
        "CourseId": course_id
    }

def seed_modules():
    """Seed modules for existing courses"""
    courses = get_existing_courses()
    
    if not courses:
        print("No courses found. Please seed courses first.")
        return

    print(f"Found {len(courses)} existing courses. Seeding modules...")
    
    for course in courses:
        course_id = course["courseId"]
        print(f"\nSeeding modules for course: {course['title']} (ID: {course_id})")
        
        for i in range(MODULES_PER_COURSE):
            module = generate_module(course_id)
            
            try:
                response = requests.post(
                    MODULE_ADD_URL,
                    data=module,
                    headers={"Content-Type": "application/x-www-form-urlencoded"},
                    verify=False
                )
                
                if response.status_code == 200:
                    print(f"✅ Module {i+1}/{MODULES_PER_COURSE}: '{module['Title']}' added successfully")
                else:
                    print(f"❌ Failed to add module: {response.text}")
                
            except Exception as e:
                print(f"🔥 Error adding module: {str(e)}")
            
            time.sleep(0.5)  # Gentle delay between requests

if __name__ == "__main__":
    seed_modules()