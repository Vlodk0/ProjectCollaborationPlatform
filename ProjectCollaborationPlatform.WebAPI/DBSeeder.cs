using ProjectCollaborationPlatform.DAL.Data.DataAccess;
using ProjectCollaborationPlatform.DAL.Data.Models;

namespace ProjectCollaborationPlatform.WebAPI
{
    public class DBSeeder
    {
        public readonly ProjectPlatformContext _context;

        public DBSeeder(ProjectPlatformContext context)
        {
            _context = context;
        }

        public void SeedTechnologies()
        {
            var technologies = new List<Technology>
            {
                new Technology { Language = "C#", Framework = "Entity Framework" },
                new Technology { Language = "C#", Framework = "ASP.NET Core" },
                new Technology { Language = "C#", Framework = "Xamarin" },
                new Technology { Language = "C#", Framework = "Unity" },
                new Technology { Language = "C#", Framework = "Blazor" },
                new Technology { Language = "C#", Framework = "Windows Presentation Foundation (WPF)" },
                new Technology { Language = "C#", Framework = "Windows Communication Foundation (WCF)" },

                new Technology { Language = "Java", Framework = "Spring Framework" },
                new Technology { Language = "Java", Framework = "Hibernate" },
                new Technology { Language = "Java", Framework = "Apache Struts" },
                new Technology { Language = "Java", Framework = "JavaServer Faces (JSF)" },
                new Technology { Language = "Java", Framework = "Apache Wicket" },
                new Technology { Language = "Java", Framework = "Play Framework" },
                new Technology { Language = "Java", Framework = "Dropwizard" },
                new Technology { Language = "Java", Framework = "Vert.x" },
                new Technology { Language = "Java", Framework = "Quarkus" },
                new Technology { Language = "Java", Framework = "Micronaut" },

                new Technology { Language = "JavaScript", Framework = "Node.js" },
                new Technology { Language = "JavaScript", Framework = "Express.js" },
                new Technology { Language = "JavaScript", Framework = "React.js" },
                new Technology { Language = "JavaScript", Framework = "Vue.js" },
                new Technology { Language = "JavaScript", Framework = "Angular" },
                new Technology { Language = "JavaScript", Framework = "Electron" },

                new Technology { Language = "Python", Framework = "Django" },
                new Technology { Language = "Python", Framework = "Flask" },
                new Technology { Language = "Python", Framework = "Pyramid" },
                new Technology { Language = "Python", Framework = "FastAPI" },
                new Technology { Language = "Python", Framework = "Tornado" },

                new Technology { Language = "PHP", Framework = "Laravel" },
                new Technology { Language = "PHP", Framework = "Symfony" },
                new Technology { Language = "PHP", Framework = "CodeIgniter" },
                new Technology { Language = "PHP", Framework = "Zend Framework" },
                new Technology { Language = "PHP", Framework = "CakePHP" },

                new Technology { Language = "C++", Framework = "Qt" },
                new Technology { Language = "C++", Framework = "Boost" },
                new Technology { Language = "C++", Framework = "STL (Standard Template Library)" },
                new Technology { Language = "C++", Framework = "OpenCV" },

                new Technology { Language = "SQL", Framework = "SQL" },

                new Technology { Language = "HTML/CSS", Framework = "HTML/CSS" },

                new Technology { Language = "Go", Framework = "Gin" },
                new Technology { Language = "Go", Framework = "Echo" },
                new Technology { Language = "Go", Framework = "Beego" },
                new Technology { Language = "Go", Framework = "Buffalo" },
                new Technology { Language = "Go", Framework = "Gorilla" },

                new Technology { Language = "Kotlin", Framework = "Spring Boot" },
                new Technology { Language = "Kotlin", Framework = "Ktor" },
                new Technology { Language = "Kotlin", Framework = "Android" },
                new Technology { Language = "Kotlin", Framework = "Micronaut" },
                new Technology { Language = "Kotlin", Framework = "Quarkus" },

            };

            _context.Technologies.AddRange(technologies);
            _context.SaveChanges();
        }
    }
}
