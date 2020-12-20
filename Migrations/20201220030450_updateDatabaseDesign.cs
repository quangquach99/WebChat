using Microsoft.EntityFrameworkCore.Migrations;

namespace WebChat.Migrations
{
    public partial class updateDatabaseDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageConversation");

            migrationBuilder.DropTable(
                name: "Sender");

            migrationBuilder.AddColumn<int>(
                name: "ConversationID",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Message",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Message");

            migrationBuilder.CreateTable(
                name: "MessageConversation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationID = table.Column<int>(type: "int", nullable: false),
                    MessageID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageConversation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MessageConversation_Conversation_ConversationID",
                        column: x => x.ConversationID,
                        principalTable: "Conversation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageConversation_Message_MessageID",
                        column: x => x.MessageID,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sender",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sender", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sender_Message_MessageID",
                        column: x => x.MessageID,
                        principalTable: "Message",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sender_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageConversation_ConversationID",
                table: "MessageConversation",
                column: "ConversationID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageConversation_MessageID",
                table: "MessageConversation",
                column: "MessageID");

            migrationBuilder.CreateIndex(
                name: "IX_Sender_MessageID",
                table: "Sender",
                column: "MessageID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sender_UserID",
                table: "Sender",
                column: "UserID",
                unique: true);
        }
    }
}
