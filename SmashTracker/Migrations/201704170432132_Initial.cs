namespace SmashTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Match",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RuleSet = c.Int(nullable: false),
                        Asterisk = c.Boolean(nullable: false),
                        Created = c.DateTime(),
                        UserCreated = c.String(),
                        Modified = c.DateTime(),
                        UserModified = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        UserDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MatchTeam",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MatchId = c.Int(nullable: false),
                        KillCount = c.Int(nullable: false),
                        Placement = c.Int(nullable: false),
                        Created = c.DateTime(),
                        UserCreated = c.String(),
                        Modified = c.DateTime(),
                        UserModified = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        UserDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Match", t => t.MatchId, cascadeDelete: true)
                .Index(t => t.MatchId);
            
            CreateTable(
                "dbo.MatchPlayer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        Character = c.Int(nullable: false),
                        Created = c.DateTime(),
                        UserCreated = c.String(),
                        Modified = c.DateTime(),
                        UserModified = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        UserDeleted = c.String(),
                        MatchTeam_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.MatchTeam", t => t.MatchTeam_Id)
                .Index(t => t.PlayerId)
                .Index(t => t.MatchTeam_Id);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        PlayerTag = c.String(maxLength: 10),
                        Created = c.DateTime(),
                        UserCreated = c.String(),
                        Modified = c.DateTime(),
                        UserModified = c.String(),
                        Deleted = c.Boolean(nullable: false),
                        UserDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Name, t.PlayerTag }, unique: true, name: "NameTag");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchPlayer", "MatchTeam_Id", "dbo.MatchTeam");
            DropForeignKey("dbo.MatchPlayer", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.MatchTeam", "MatchId", "dbo.Match");
            DropIndex("dbo.Player", "NameTag");
            DropIndex("dbo.MatchPlayer", new[] { "MatchTeam_Id" });
            DropIndex("dbo.MatchPlayer", new[] { "PlayerId" });
            DropIndex("dbo.MatchTeam", new[] { "MatchId" });
            DropTable("dbo.Player");
            DropTable("dbo.MatchPlayer");
            DropTable("dbo.MatchTeam");
            DropTable("dbo.Match");
        }
    }
}
