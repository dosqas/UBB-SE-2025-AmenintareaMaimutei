// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Drug.cs" company="YourCompanyName">
//   Copyright (c) YourCompanyName. All rights reserved.
// </copyright>
// <summary>
//   Defines the Drug class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Project.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a drug with its details such as ID, name, administration method, specification, and supply.
    /// </summary>
    public class Drug
    {
        /// <summary>
        /// Gets or sets the unique identifier for the drug.
        /// </summary>
        public int DrugID { get; set; }

        /// <summary>
        /// Gets or sets the name of the drug.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the method of administration for the drug.
        /// </summary>
        public string Administration { get; set; }

        /// <summary>
        /// Gets or sets the specification of the drug.
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// Gets or sets the supply quantity of the drug.
        /// </summary>
        public int Supply { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class with default values.
        /// </summary>
        public Drug()
        {
            this.Name = "Unspecified";
            this.Administration = "0";
            this.Specification = "0";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class with specified values.
        /// </summary>
        /// <param name="drugID">The unique identifier for the drug.</param>
        /// <param name="name">The name of the drug.</param>
        /// <param name="administration">The method of administration for the drug.</param>
        /// <param name="specification">The specification of the drug.</param>
        /// <param name="supply">The supply quantity of the drug.</param>
        public Drug(int drugID, string name, string administration, string specification, int supply)
        {
            this.DrugID = drugID;
            this.Name = name;
            this.Administration = administration;
            this.Specification = specification;
            this.Supply = supply;
        }
    }
}
