using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuggestionAppUI.Components.Pages
{
    public partial class SampleData
    {



        private bool categoriesCreated = false;
        private bool statusesCreated = false;

        /// <summary>
        /// Method to generate sample data
        /// </summary>
        /// <returns></returns>
        private async Task GenerateSampleData()
        {
            UserModel user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john@test.com",
                DisplayName = "JohnDoe",
                ObjectIdentifier = "1234"
            };
            await userData.CreateUser(user);
            var foundUser = await userData.GetUserFromAuthentication("1234");
            var categories = await categoryData.GetAllCategories();
            var statuses = await statusData.GetAllStatuses();
            HashSet<string> votes = new();
            votes.Add("1234");
            votes.Add("2");
            votes.Add("3");
            votes.Add("4");

            SuggestionModel suggestion = new()
            {
                Author = new BasicUserModel(foundUser),
                Category = categories[0],
                Suggestion = "First suggestion",
                Description = "This is the first suggestion created by the sample data geneartion method",
                UserVotes = votes,
            };
            await suggestionData.CreateSuggestion(suggestion);
            votes.Add("5");
            suggestion = new()
            {
                Author = new BasicUserModel(foundUser),
                Category = categories[0],
                Suggestion = "Second suggestion",
                Description = "This is the second suggestion created by the sample data geneartion method",
                SuggestionStatus = statuses[0],
                UserVotes = votes,
                OwnerNotes = "This is a note for the owner"
            };
            await suggestionData.CreateSuggestion(suggestion);
            suggestion = new()
            {
                Author = new BasicUserModel(foundUser),
                Category = categories[2],
                Suggestion = "Third suggestion",
                Description = "This is the third suggestion created by the sample data geneartion method",
                SuggestionStatus = statuses[1],
                OwnerNotes = "This is a note for the owner"
            };
            await suggestionData.CreateSuggestion(suggestion);
            suggestion = new()
            {
                Author = new BasicUserModel(foundUser),
                Category = categories[5],
                Suggestion = "Fourth suggestion",
                Description = "This is the fourth suggestion created by the sample data geneartion method",
                SuggestionStatus = statuses[3],
                OwnerNotes = "This is a note for the owner"
            };
            await suggestionData.CreateSuggestion(suggestion);
        }

        /// <summary>
        /// Categories hard-coded for version1 of the app.
        /// To-do : allow admins to create, update, delete categories
        /// </summary>
        /// <returns></returns>
        private async Task CreateCategories()
        {

            var categories = await categoryData.GetAllCategories();
            //categories? ==> if not null
            if (categories?.Count > 0)
            {
                return;
            }
            CategoryModel

            cat = new()
            {
                CategoryName = "Fiction",
                CategoryDescription = "Fiction refers to literature that is not grounded in real-life events but is made up or created from a writer's imagination."
            };
            await categoryData.CreateCategory(cat);

            cat = new()
            {
                CategoryName = "Nonfiction",
                CategoryDescription = "Nonfiction refers to factual stories based on real people, information, or events."
            };
            await categoryData.CreateCategory(cat);

            cat = new()
            {
                CategoryName = "Science Fiction",
                CategoryDescription = "Science Fiction Books deal with imaginative and futuristic concepts such as advanced science and technology, space exploration, time travel, parallel universes, and extraterrestrial life"
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Historical Fiction",
                CategoryDescription = "Historical Fiction is a story that takes readers to a time and place in the past."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Mystery",
                CategoryDescription = "The mystery is a genre of literature whose stories focus on a puzzling crime, situation, or circumstance that needs to be solved."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Romance",
                CategoryDescription = "Romance novels are typically about a romantic relationship. Typically the would-be lovers are faced by a series of obstacles that keep them apart for most of the book."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Thriller",
                CategoryDescription = "Thriller fiction is a genre of crime fiction that focuses on suspense and psychological thrillers. Thrillers often feature complex plots and characters who are psychologically damaged or mentally unstable."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Autobiography",
                CategoryDescription = "An autobiography is defined as a book describing the author's personal experiences."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Adventure",
                CategoryDescription = "Any novel that focuses on an adventure undertaken by the main character (with or without help) falls under the adventure genre"
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Self-help",
                CategoryDescription = "Self-help books provide advice and help readers solve personal problems or achieve specific goals. Self-help topics include building confidence, relationship advice, financial management, dieting or business success. "
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Religion and spirituality",
                CategoryDescription = "Religion and spirituality nonfiction can take multiple forms. Some books describe the author's personal experience, some are theory-based and others are self-help books for readers wanting to grow their spirituality."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Philosophy",
                CategoryDescription = "Philosophy nonfiction analyzes concepts such as ethics and freedom and invites readers to think about them. It also explains philosophical paradigms to beginners who want to learn about them and leads them to experts on the topics."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Business",
                CategoryDescription = "Business books often serve as guides about management and entrepreneurship. The authors of this genre have experience in the market or industry and share their tips with readers."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Health and wellness",
                CategoryDescription = "Health and wellness books cover topics such as stress management, sleeping habits or diet. They can address physical, mental or spiritual health"
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Language",
                CategoryDescription = "Language books teach people how to communicate in a new language. Some language books also explain the origins and functions of tongues or dialects."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Food writing",
                CategoryDescription = "Food writing can include: Cookbooks, Individual recipes, Restaurant guidebooks, Cultural studies of various cuisines around the world, Personal essays or memoirs related to the author's memories of food, Historical nonfiction about food, such as how people used or traded a particular food throughout a historical era, Modified recipes, such as for dietary restrictions or a different cooking method, Reviews of restaurants or recipes"
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Cultural criticism",
                CategoryDescription = "Cultural criticism focuses on analyzing or offering a unique take on some aspect of modern culture. This aspect can be an array of things, including a product, social trend, political movement or film."
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Textbooks",
                CategoryDescription = "Textbooks provide comprehensive information to help readers learn about a particular subject. They're usually part of a curriculum and include questions that encourage learners to review the book's subject. "
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Craft and hobby",
                CategoryDescription = "Craft and hobby nonfiction answers questions readers might have about their interests. Examples of craft and hobby book topics include calligraphy, crochet, gardening or origami. "
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Political and social science",
                CategoryDescription = "Political and social science books analyze and comment on a particular attribute or structure in society"
            };
            await categoryData.CreateCategory(cat);
            cat = new()
            {
                CategoryName = "Science",
                CategoryDescription = "Science books describe academic research and might include technical information. Science nonfiction writing is well-organized and follows academic conventions such as indexing and referencing. It also includes an appendix of scientific proof to support the writer's statements and further the reader's knowledge."
            };
            await categoryData.CreateCategory(cat);

            categoriesCreated = true;

        }
        /// <summary>
        /// Statuses of the suggestions creation
        /// </summary>
        /// <returns></returns>
        private async Task CreateStatuses()
        {
            var statuses = await statusData.GetAllStatuses();
            if (statuses?.Count > 0)
            {
                return;
            }
            StatusModel stat = new()
            {
                StatusName = "Completed",
                StatusDescription = "The suggestion was accepted and released."
            };
            await statusData.CreateStatus(stat);
            stat = new()
            {
                StatusName = "Watching",
                StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
            };
            await statusData.CreateStatus(stat);
            stat = new()
            {
                StatusName = "Upcoming",
                StatusDescription = "The suggestion was accepted and it will be handled soon."
            };
            await statusData.CreateStatus(stat);
            stat = new()
            {
                StatusName = "Dismissed",
                StatusDescription = "The suggestion was not something that we are going to undertake."
            };
            await statusData.CreateStatus(stat);
            statusesCreated = true;
        }
    }
}