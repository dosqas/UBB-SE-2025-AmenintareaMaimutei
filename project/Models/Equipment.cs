namespace Project.Models
{
    /// <summary>
    /// Equipment class represents equipment in a hospital or clinic.
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class.
        /// </summary>
        public Equipment()
        {
            this.Name = "Unspecified";
            this.Type = "Unspecified";
            this.Specification = "Unspecified";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class with specified equipment ID, name, type, specification, and stock.
        /// </summary>
        /// <param name="equipmentID">The unique identifier for the equipment.</param>
        /// <param name="name">The name of the equipment.</param>
        /// <param name="type">The type of the equipment.</param>
        /// <param name="specification">The specification of the equipment.</param>
        /// <param name="stock">The stock of the equipment.</param>
        public Equipment(int equipmentID, string name, string type, string specification, int stock)
        {
            this.EquipmentID = equipmentID;
            this.Name = name;
            this.Type = type;
            this.Specification = specification;
            this.Stock = stock;
        }

        /// <summary>
        /// Gets or Sets the equipment ID.
        /// </summary>
        public int EquipmentID { get; set; }

        /// <summary>
        /// Gets or Sets the name of the equipment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the type of the equipment.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the specification of the equipment.
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// Gets or Sets the stock of the equipment.
        /// </summary>
        public int Stock { get; set; }
    }
}
