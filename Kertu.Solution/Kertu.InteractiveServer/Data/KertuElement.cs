﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kertu.InteractiveServer.Data
{
    /// <summary>
    /// Base class for future Kertu elements
    /// </summary>
    /// <remarks>
    /// All Kertu elements such as lists or cards should inherit this class
    /// </remarks>
    public class KertuElement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// </summary>
        [ForeignKey(nameof(ApplicationUser))]
        [Column(TypeName = "varchar(255)")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }


        [Column(TypeName = "varchar(255)")]
        public string? OwnerId { get; set; }
        public virtual ApplicationUser? Owner { get; set; }
    }
}
