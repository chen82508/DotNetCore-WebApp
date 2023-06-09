﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HR.Models.Entity
{
    [Table("SY_OPTION", Schema = "dbo")]
    public partial class SY_OPTION
    {
        [Key]
        public int SY_OPT_SN { get; set; }
        public int SY_OPT_SOT_SN { get; set; }
        [Required]
        [StringLength(50)]
        public string SY_OPT_NAM { get; set; }
        [Required]
        [StringLength(1)]
        [Unicode(false)]
        public string SY_OPT_ACT_TYP { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SY_OPT_ADD_DAT { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SY_OPT_UPD_DAT { get; set; }

        [ForeignKey("SY_OPT_SOT_SN")]
        [InverseProperty("SY_OPTION")]
        public virtual SY_OPTION_TAG SY_OPT_SOT_SNNavigation { get; set; }
    }
}