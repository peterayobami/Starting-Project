using Swashbuckle.AspNetCore.Filters;

namespace Starting_Project
{
    public class ApplicationCredentialsModelExample : IExamplesProvider<ApplicationCredentials>
    {
        public ApplicationCredentials GetExamples()
        {
            return new ApplicationCredentials
            {
                ProgramId = "1f171db7-38b4-4555-85d7-db3efc720b13",
                FirstName = "James",
                LastName = "Olayemi",
                Email = "james.olayemi@hotmail.com",
                Phone = "08099898876",
                Nationality = "Nigeria",
                CurrentResidence = "Nigeria",
                IdNumber = "121562908",
                DateOfBirth = "1998-03-03",
                Gender = "MALE",
                PersonalAnswers =
                [
                    new OtherAnswer
                    {
                        Text = "6.2"
                    }
                ],
                CustomAnswers =
                [
                    new OtherAnswer
                    {
                        Text = "NO"
                    },
                    new OtherAnswer
                    {
                        MultipleChioces = ["creative", "outgoing", "incoming"]
                    },
                    new OtherAnswer
                    {
                        Number = 5
                    },
                    new OtherAnswer
                    {
                        Text = "2011-06-06"
                    },
                ]
            };
        }
    }
}
