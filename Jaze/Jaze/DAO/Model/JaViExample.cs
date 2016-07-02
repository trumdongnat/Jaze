﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Jaze.DAO.Model
{
    [Table("JaviExam")]
    public class JaViExample
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Column("ja")]
        public string Japanese { get; set; }

        [Column("vi")]
        public string VietNamese { get; set; }
    }
}