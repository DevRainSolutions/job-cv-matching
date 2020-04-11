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

        //AI results
        public double Score { get; set; }
        public string ChartResult { get; set; }
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

                CV = TemplateResume.Replace("                                    ","").Replace("	","");
                JobDescription = sample.JobDescription.Replace("                                    ", "").Replace("	", ""); ;

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
                ChartResult = await chartResult.Content.ReadAsStringAsync();
                var score = JsonSerializer.Deserialize<ChartModel>(ChartResult).metrics.weighted_jaccard;
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


        private string TemplateResume = @"Oleksandr Krakovetskyi
                                    Software Engineer and Architect, AI/Data Science Expert, Ph.D.
                                    Key awards, projects and experience:
                                    -	2019. Microsoft Certified: Azure Data Scientist Associate
                                    -	2019. Co-author of “Exam DP-100: Designing and Implementing a Data Science Solution on Azure” certification exam for Microsoft
                                    -	2019. Co-author of “Web Development Foundations” certification exam for WGU
                                    -	AI Lead in Inphiz (Sweden, from 2018)
                                    -	CEO, co-founder of DevRain (from 2012)
                                    -	CTO, co-founder of DonorUA, a “GPS” service for blood donors (from 2015)
                                    -	CEO, co-founder of Chatty.ai, an automated platform for the employees interviewing, onboarding, learning and testing (own project, from 2019)
                                    -	Compliance check software for Sweden law firm, from 2018
                                    -	2018. “AI fundamentals” training for Microsoft partners (Minsk, Belarus)
                                    -	2018. Chatbot platform development for Social27 (Seattle, USA)
                                    -	2018. Software for creating e-books (own project)
                                    -	2018. Adviser in RevDebug (Poland)
                                    -	Awarded as “Microsoft Most Valuable Professional” in Artificial Intelligence (from 2011)
                                    -	Awarded as “Microsoft Regional Director” (from 2010)
                                    -	Awarded as “Telerik Developer Expert” (2013-2015)
                                    -	Awarded as “The Best Professional in Software Architecture” (Ukrainian IT Awards, 2013)
                                    -	2011. Ph.D. in Computer Science (Ukraine)
                                    -	Expert in Open Data in State Agency for E-Governance of Ukraine (2015-2018)
                                    -	Community lead in Kyiv Smart City initiative (2015-2016)
                                    -	Jury member in multiple startup initiatives
                                    -	EGAP Challenge national coordinator, a competition for e-democracy and open data projects, 2016-2017
                                    -	Speaker and writer, author of 1 book and 20 science papers

                                    Technologies and professional skills:
                                    -	AI: Data Mining, Data Extracting, Natural Language Processing, Named Entity Recognition, Math, Machine Learning, Numerical Methods, Math Modelling, Azure Machine Learning
                                    -	Chatbots: Microsoft Bot Framework, Azure Cognitive Services, Chatfuel, WIT.AI, LUIS
                                    -	.NET: WPF, WinForms, Mono
                                    -	Enterprise and cloud technologies: Microsoft Azure, Office 365
                                    -	Web: ASP.NET MVC, Entity Framework, Web API, REST, JavaScript
                                    -	Mobile: Windows Phone, Windows Unified Platform, iOS, Android, Xamarin
                                    -	Domains: Open Data, Smart City, eHealth, Semantic Web
                                    -	Project management: Agile/Scrum, Jira, Asana, Redmine, TFS, Visual Studio Online, Redmine, UML, Mercurial
                                    -	DB: NoSQL, MySQL, Microsoft SQL Server, SQL Azure, SQLite
                                    -	Tools: XCode, Microsoft Visual Studio, NUnit, JetBrains ReSharper, Telerik tools, Red Gate tools, NDepend, StyleCop, FxCop, SharpDevelop";

        private List<Template> Templates = new List<Template>()
        {
            new Template()
            {
                Id = 1,

                JobDescription = @"We are hungry to hire mature Data Scientists to optimize existing trading signals and identify new alpha signals. The following attributes are in demand;
                                    - A minimum of 3 years exposure with ML, Natural Language Processing, Named Entity Recognition, AI, Deep Learning and Reinforcement algorithms and their corresponding feature engineering efforts.
                                    - Passion for performing data mining methods on alternative data sets to optimize, amplify and retain revenues with actionable insights.
                                    - Superior Python/.NET coding skills
                                    - STEM PhD with post-doc experience focusing on building data engineering tools, customizing machine learning algorithms and a collaborative mindset."
            },

            new Template()
            {
                Id = 2,

                JobDescription = @"Responsibilities:
                                    - Participate in requirements gathering, technical specification, and the design and development of complex software projects
                                    - Work with product managers, content producers, QA engineers and release engineers to own your solution from development to production
                                    - Contribute to software architecture design, development of software applications, and integration into enterprise systems
                                    - Design software architecture based on business requirements, strategy and priorities

                                    Requirements:
                                    - BS/MS in Computer Science or equivalent experience and evidence of exceptional ability
                                    - 2+ years of working experience
                                    - Experience in .NET Framework and .NET Core
                                    - Database experience (MySQL, NoSSQL, MsSQL Server) and good knowledge in query optimization
                                    - Experience with distributed architectures and REST APIs
                                    - Good unit testing and integration testing practices
                                    - Excellent interpersonal communication skills"
            },

            new Template()
            {
                Id = 3,

                JobDescription = @"- Bachelor’s degree in Computer Science, Computer Engineering, Technology, Information Systems (CIS/MIS), Engineering or related technical discipline, or equivalent experience/training
                                    - 5 years of full Software Development Life Cycle (SDLC) experience
                                    - 3+ years of Designed and developed front-end application
                                    - Proficiency and demonstrated experience in the following technologies:
                                    - 5 years’ experience in J2EE technologies: Java, JSP, JMS, JAXB, JDBC
                                    - Database and persistence frameworks: Hibernate, Oracle, Object/Relational Mapping, Query performance tuning
                                    - Web Servers: Tomcat, tcServer, Websphere
                                    - Web Services: REST/SOAP (JSON/WSDL/XML)
                                    - Web: Angular, AngularJS, Ext JS, HTML/5, CSS, jQuery, AJAX, JavaScript, MVC framework
                                    - Proficient in Spring MVC, Spring-taglib, Spring Web Flow and/or Spring Mobile
                                    - Proficient in Responsive web design understanding (css media queries)
                                    - Build/deployment tools: Maven, Git, , Junit, Mockito
                                    - Proficiency in object-oriented design techniques and principles
                                    - Proficiency in Microsoft Office Tools (Project, Excel, Word, PowerPoint, etc.)
                                    - Proficient in CSS3, SASS/SCSS/LESS/COMPASS implementation and best practices
                                    - Experience in Agile methodologies, such as SCRUM
                                    - Ability to explain technical concepts and adjust messaging based on the audience, including non-technical groups
                                    - Ability to influence through outstanding interpersonal skills, collaboration, and negotiation skills
                                    - Ability to work well within a team environment, as well as independently"
            }
        };
    }
}
