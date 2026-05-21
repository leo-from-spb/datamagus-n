using System.Collections.Generic;

namespace Util.Text;


[TestFixture]
public class TextTableTest
{

    [Test]
    public void ProcessContent_OneColumn_Mono()
    {
        string[] content = ["Eins", "Zwei", "Drei", "Vier"];
        var tt =
            new TextTable<string>()
                .Column("Zahl", s => s);
        string text = tt.ProcessContent(content);
        text.ShouldBe("""
                      ====
                      Zahl
                      ----
                      Eins
                      Zwei
                      Drei
                      Vier
                      ----

                      """);
    }


    [Test]
    public void ProcessContent_OneColumn_MonoBox()
    {
        string[] content = ["Eins", "Zwei", "Drei", "Vier"];
        var tt =
            new TextTable<string>()
                .Column("Zahl", s => s)
                .WithGrid(new TextTableGrid(LeftLine: "! ", RightLine: " !"));
        string text = tt.ProcessContent(content);
        text.ShouldBe("""
                      ========
                      ! Zahl !
                      --------
                      ! Eins !
                      ! Zwei !
                      ! Drei !
                      ! Vier !
                      --------

                      """);
    }


    [Test]
    public void ProcessContent_TwoColumns_Mono()
    {
        var content = new Dictionary<int, string>
                      {
                          [1] = "Eins",
                          [2] = "Zwei",
                          [3] = "Drei",
                          [4] = "Vier"
                      };
        var tt =
            new TextTable<KeyValuePair<int, string>>()
                .Column("#",    r => r.Key.ToString())
                .Column("Zahl", r => r.Value)
                .WithGrid(new TextTableGrid(CellGap: " ! "));
        string text = tt.ProcessContent(content);
        text.ShouldBe("""
                      ========
                      # ! Zahl
                      --------
                      1 ! Eins
                      2 ! Zwei
                      3 ! Drei
                      4 ! Vier
                      --------

                      """);
    }


    [Test]
    public void ProcessContent_TwoColumns_Left()
    {
        string[] content = ["Apfel", "Birne", "Kiwi", "Mandarine", "Johannisbeere", "Mango"];
        var tt =
            new TextTable<string>()
                .Column("Obst", TextColumnAlign.tcaLeft, s => s)
                .Column("#",    TextColumnAlign.tcaLeft, s => s.Length.ToString())
                .WithGrid(new TextTableGrid(LeftLine: "! ", CellGap: " ! ", RightLine: " !"));
        string text = tt.ProcessContent(content);
        text.ShouldBe("""
                      ======================
                      ! Obst          ! #  !
                      ----------------------
                      ! Apfel         ! 5  !
                      ! Birne         ! 5  !
                      ! Kiwi          ! 4  !
                      ! Mandarine     ! 9  !
                      ! Johannisbeere ! 13 !
                      ! Mango         ! 5  !
                      ----------------------

                      """);
    }


    [Test]
    public void ProcessContent_TwoColumns_Right()
    {
        string[] content = ["Apfel", "Birne", "Kiwi", "Mandarine", "Johannisbeere", "Mango"];
        var tt =
            new TextTable<string>()
                .Column("Obst", TextColumnAlign.tcaRight, s => s)
                .Column("#",    TextColumnAlign.tcaRight, s => s.Length.ToString())
                .WithGrid(new TextTableGrid(LeftLine: "! ", CellGap: " ! ", RightLine: " !"));
        string text = tt.ProcessContent(content);
        text.ShouldBe("""
                      ======================
                      !          Obst !  # !
                      ----------------------
                      !         Apfel !  5 !
                      !         Birne !  5 !
                      !          Kiwi !  4 !
                      !     Mandarine !  9 !
                      ! Johannisbeere ! 13 !
                      !         Mango !  5 !
                      ----------------------

                      """);
    }

}
