using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionsAndAnswersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Polls_PollId",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "Answers");

            migrationBuilder.RenameIndex(
                name: "IX_Question_PollId_content",
                table: "Questions",
                newName: "IX_Questions_PollId_content");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_QuestionId_content",
                table: "Answers",
                newName: "IX_Answers_QuestionId_content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answers",
                table: "Answers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Polls_PollId",
                table: "Questions",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Polls_PollId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answers",
                table: "Answers");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "Answers",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_PollId_content",
                table: "Question",
                newName: "IX_Question_PollId_content");

            migrationBuilder.RenameIndex(
                name: "IX_Answers_QuestionId_content",
                table: "Answer",
                newName: "IX_Answer_QuestionId_content");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_QuestionId",
                table: "Answer",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Polls_PollId",
                table: "Question",
                column: "PollId",
                principalTable: "Polls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
