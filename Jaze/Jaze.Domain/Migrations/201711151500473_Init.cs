namespace Jaze.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Init : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.grammar",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Struct = c.String(maxLength: 4000),
            //            Meaning = c.String(maxLength: 4000),
            //            Detail = c.String(maxLength: 4000),
            //            Level = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.hanviet",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            Reading = c.String(maxLength: 4000),
            //            Content = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.jaenExam",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ja = c.String(maxLength: 4000),
            //            en = c.String(maxLength: 4000),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.jaen",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            Kana = c.String(maxLength: 4000),
            //            Mean = c.String(maxLength: 4000),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.JaviExam",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false),
            //            ja = c.String(maxLength: 4000),
            //            vi = c.String(maxLength: 4000),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.javi",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            Kana = c.String(maxLength: 4000),
            //            Mean = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.kanji",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            HanViet = c.String(maxLength: 4000),
            //            Variant = c.String(maxLength: 4000),
            //            Onyomi = c.String(maxLength: 4000),
            //            Kunyomi = c.String(maxLength: 4000),
            //            VieMeaning = c.String(maxLength: 4000),
            //            EngMeaning = c.String(maxLength: 4000),
            //            Stroke = c.Int(nullable: false),
            //            StrokeOrder = c.String(maxLength: 4000),
            //            Frequence = c.Int(nullable: false),
            //            Similar = c.String(maxLength: 4000),
            //            Component = c.String(maxLength: 4000),
            //            Level = c.Int(nullable: false),
            //            Grade = c.Int(nullable: false),
            //            Radical_Id = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.radical", t => t.Radical_Id)
            //    .Index(t => t.Radical_Id);

            //CreateTable(
            //    "dbo.part",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Value = c.String(maxLength: 4000),
            //            Word = c.String(maxLength: 4000),
            //            Stroke = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.radical",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            HanViet = c.String(maxLength: 4000),
            //            Meaning = c.String(maxLength: 4000),
            //            Stroke = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.vija",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Word = c.String(maxLength: 4000),
            //            Mean = c.String(maxLength: 4000),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Kanji_Part",
            //    c => new
            //        {
            //            KanjiId = c.Int(name: "KanjiId ", nullable: false),
            //            PartId = c.Int(name: "PartId ", nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.KanjiId, t.PartId })
            //    .ForeignKey("dbo.kanji", t => t.KanjiId, cascadeDelete: true)
            //    .ForeignKey("dbo.part", t => t.PartId, cascadeDelete: true)
            //    .Index(t => t.KanjiId, name: "IX_KanjiId")
            //    .Index(t => t.PartId, name: "IX_PartId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.kanji", "Radical_Id", "dbo.radical");
            DropForeignKey("dbo.Kanji_Part", "PartId ", "dbo.part");
            DropForeignKey("dbo.Kanji_Part", "KanjiId ", "dbo.kanji");
            DropIndex("dbo.Kanji_Part", "IX_PartId");
            DropIndex("dbo.Kanji_Part", "IX_KanjiId");
            DropIndex("dbo.kanji", new[] { "Radical_Id" });
            DropTable("dbo.Kanji_Part");
            DropTable("dbo.vija");
            DropTable("dbo.radical");
            DropTable("dbo.part");
            DropTable("dbo.kanji");
            DropTable("dbo.javi");
            DropTable("dbo.JaviExam");
            DropTable("dbo.jaen");
            DropTable("dbo.jaenExam");
            DropTable("dbo.hanviet");
            DropTable("dbo.grammar");
        }
    }
}