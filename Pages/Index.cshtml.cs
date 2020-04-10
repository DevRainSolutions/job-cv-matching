using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public string TempId { get; set; }

        [BindProperty]
        public string CV { get; set; }

        [BindProperty]
        public string JobDescription { get; set; }
        public object JsonConvert { get; private set; }

        public double Score { get; set; }
        public string GraphResult { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            if(TempId != null) 
            {
                var sample = Templates.FirstOrDefault(x => x.Id == int.Parse(TempId));
                if (sample == null) sample = Templates.FirstOrDefault();

                CV = sample.CV;
                JobDescription = sample.JobDescription;

                await Analyze();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await Analyze();

            return Page();
        }

        private async Task Analyze() 
        {
            var client = new HttpClient();

            var content = JsonSerializer.Serialize(new
            {
                job = JobDescription,
                cv = CV
            });

            var chartResult = client.PostAsync("https://chatty-api.azurewebsites.net/compare_charts", new StringContent(content, Encoding.UTF8, "application/json")).Result;
            var graphResult = client.PostAsync("https://chatty-api.azurewebsites.net/graph", new StringContent(content, Encoding.UTF8, "application/json")).Result;

            if (chartResult.IsSuccessStatusCode)
            {
                var res = await chartResult.Content.ReadAsStringAsync();
                var score = JsonSerializer.Deserialize<ChartModel>(res).metrics.weighted_jaccard;
                Score = Math.Round(score, 0);
            }

            if (graphResult.IsSuccessStatusCode)
            {
                GraphResult = await graphResult.Content.ReadAsStringAsync();
            }
        }

        public class Template 
        {
            public int Id { get; set; }
            public string CV { get; set; }
            public string JobDescription { get; set; }
        }

        private List<Template> Templates = new List<Template>()
        {
            new Template()
            {
                Id = 1,

                CV = @"Lightning-fast, exacting .NET developer with 2+ years of experience. Skilled in ASP.NET MVC, and client-side web development.
                        Seeking position at Citrix. As intern at BrainWild Global, worked on 10+ projects for SQL database design
                        Developed and maintained over 20 .NET websites with 100% client satisfaction.",

                JobDescription = @"DevRain is a Ukrainian full-cycle software development company with specialization in mobile, 
                                    web, cloud and AI/ML development (Xamarin, React Native, Flutter, ASP.NET Core, Azure, iOS, Android, AI/ML, UX/UI).
                                    Working with startups and enterprises in different domains including ERP/CRM/CMS systems, entertainment/communication/media, 
                                    finance/banking, digital signage, e-Health and Smart City/eGov. Providing a full stack development from the idea to publishing, 
                                    as well as research & development, startups consulting, data analysis and other services. Company is experienced in designing and 
                                    developing intuitive desktop, mobile, tablet, wearable, TV solutions on Microsoft and iOS/Android platforms."
            },

            new Template()
            {
                Id = 2,

                CV = @"I am currently working as a Front End Developer on a small React team building Insurance applications and I am across multiple applications 
                        that have been developed using our new technology stack. IAG are keen to utilise the latest technologies and we have had great success using React/Redux
                        running on a Node Express server and using an API backend architecture (via APIGEE) to access the various IAG systems. I have also been a key part of a team
                        developing an in-house Design System called Chroma, this is a platform and framework agnostic component library. We have then built a React component library 
                        on top of this system which is used for all IAG Customer Labs applications. As part of this team I have also been responsible for setting up a CI/CD pipeline
                        using Github/CircleCI/Heroku/NPM & AWS.",


                JobDescription = @"We are currently searching for a Senior Front-end (React.js) Engineer, passionate about vision and idea of our product and able to work with user-facing 
                                    part of our application. We take inspiration from Spotify for building our engineering culture and implementing best practices. We work in small product teams. 
                                    You will join our Supply Team, responsible for attracting the best tutors and helping them advertise themselves on our platform. Our front-end
                                    developers work on all parts of our system from improving the UX, to making our API more robust. We like to build user-facing features, Typescript, TDD,
                                    A/B testing, speed optimization and SEO tricks that help us build a bigger business. We have diverse technical challenges that will allow you to develop your
                                    skills across the stack."
            },

            new Template()
            {
                Id = 3,


                CV = @"Around 6 years of experience as a Web/Application Developer and coding with analytical programming using
                        Python, Django. To serve the organization with the best of my technical skills and abilities by utilizing my
                        educational and professional knowledge and competencies acquired by me in my academic career and
                        professional experience.
                        CORE QUALIFICATION
                        • Experienced with full software development life-cycle, architecting scalable platforms, object oriented
                        programming, database design and agile methodologies
                        • Built Web application using Python, Django, Flask, JavaScript, AJAX, HTML and template languages.
                        • Used Apache to deploy production site.
                        • Strong experience using Web Services and API’s in python.
                        • Experience in using Design Patterns such as MVC and frameworks such as Django, Flask.
                        • Proficient in SQL databases MySQL, PostgreSQL, Oracle and MongoDB.",


                JobDescription = @"ougov is searching for a Remote Python Developer to collaborate building a modern and large-scale cloud-based analytics platform built for the web, entirely in Python.
                                    What will I be doing?

                                    Working on the data transformation and transmitting the data, making assurances about the integrity of it
                                    Our data flows handle more than 30 million data points every week, you will be central on the delivery of that data to our analytics team
                                    Work primarily on backend, middleware and databases but everyone in the team regularly works on the full stack.

                                    4+ years’ experience of Python web-based development
                                    Experience using CherryPy, Flask or similar framework
                                    Basic understanding of MongoDB, Django & PostgreSQL
                                    Understanding of TDD methodology
                                    An interest in ETL"
            }
        };
    }
}
