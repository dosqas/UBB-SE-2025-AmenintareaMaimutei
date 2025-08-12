import requests
from faker import Faker
import random
import time

fake = Faker()

url = "https://localhost:7174/api/Course/add"

num_courses = 10
delay_between_requests = 0.5  # seconds

difficulties = ["Beginner", "Intermediate", "Advanced"]

def generate_course():
    """Generate a fake course dictionary"""
    is_premium = random.choice([True, False])
    cost = random.randint(0, 100) if not is_premium else random.randint(100, 500)
    
    return {
        "Title": fake.sentence(nb_words=3).replace('.', ''),
        "Description": fake.paragraph(nb_sentences=3),
        "IsPremium": is_premium,
        "Cost": cost,
        "ImageUrl": fake.image_url(),
        "TimeToComplete": random.randint(1800, 86400),
        "Difficulty": random.choice(difficulties)
    }

def send_course(course_data):
    """Send course data to the API"""
    headers = {
        "Content-Type": "application/x-www-form-urlencoded"
    }
    
    try:
        response = requests.post(url, data=course_data, headers=headers, verify=False)
        if response.status_code == 200:
            print(f"✅ Success: {course_data['Title']}")
        else:
            print(f"❌ Failed (HTTP {response.status_code}): {course_data['Title']}\nResponse: {response.text}")
    except Exception as e:
        print(f"🔥 Error sending '{course_data['Title']}': {str(e)}")

def main():
    print(f"🌱 Seeding {num_courses} courses...\n")
    for i in range(num_courses):
        course = generate_course()
        print(f"📤 Sending course {i+1}/{num_courses}: {course['Title']}")
        send_course(course)
        time.sleep(delay_between_requests)
    print("\n🎉 Seeding complete!")

if __name__ == "__main__":
    main()