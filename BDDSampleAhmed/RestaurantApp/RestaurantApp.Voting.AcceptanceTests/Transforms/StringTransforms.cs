using System.Collections.Specialized;
using System.Linq;
using TechTalk.SpecFlow;

namespace RestaurantApp.Voting.AcceptanceTests.Transforms
{
    [Binding]
    public class StringTransforms
    {
        [StepArgumentTransformation]
        public StringCollection GetStrings(string nonEmptyCommaSeperatedStrings)
        {
            var strings = new StringCollection();
            strings.AddRange(nonEmptyCommaSeperatedStrings.Split(',')
                                                          .Select(_ => _.Trim())
                                                          .ToArray());

            return strings;
        }
    }
}
