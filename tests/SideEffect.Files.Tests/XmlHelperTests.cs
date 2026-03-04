using NUnit.Framework;
using SideEffect.Files.XML;
using System.Text;

namespace SideEffect.Files.Tests;

[TestFixture]
public class XmlHelperTests
{
    [TestCase(
        "  <?xml version=\"1.0\" encoding=\"UTF-8\"?><Root><Activities>  \r\n   <Activity   /> <Activity>   Name </Activity> </Activities></Root> ",
        "<?xml version=\"1.0\" encoding=\"utf-8\"?><Root><Activities><Activity /><Activity>   Name </Activity></Activities></Root>")]
    public void FirstDayOfWeekTest(string input, string expectedOutput)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);

        var outputBytes = XmlHelper.Minify(inputBytes);

        var expectedOutputBytes = Encoding.UTF8.GetBytes(expectedOutput);

        Assert.That(outputBytes, Is.EqualTo(expectedOutputBytes));
    }
}
