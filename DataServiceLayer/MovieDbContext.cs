using DataServiceLayer.Domains;
using DataServiceLayer.ReadDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLayer;

public class MovieDbContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<TitleRating> TitleRatings { get; set; }
    public DbSet<PersonRating> PersonRatings { get; set; }
    public DbSet<Casting> Castings { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Profession> Professions { get; set; }
    public DbSet<TitleType> TitleTypes { get; set; }
    public DbSet<TitleAka> TitleAkas { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Search> Searches { get; set; }
    public DbSet<BookmarkPerson> BookmarkPersons { get; set; }
    public DbSet<BookmarkTitle> BookmarkTitles { get; set; }

    public DbSet<TitleReadDto> TitleReadDtos { get; set; }


    // connect to db
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // show full query details in console
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();

        // Remember to set the environment variable PG_PASSWORD before running the application
        string password = Environment.GetEnvironmentVariable("PG_PASSWORD");
        string connectionString = $"host=localhost;db=imdb;uid=postgres;pwd={password}";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        /////////////////////////////////////////////////////////
        ///                      IMDB DATA                    ///                   
        /////////////////////////////////////////////////////////

        // map Casting to casting   
        modelBuilder.Entity<Casting>().ToTable("casting");
        modelBuilder.Entity<Casting>().Property(c => c.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<Casting>().Property(c => c.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<Casting>().Property(c => c.CharacterName).HasColumnName("character_name");
        modelBuilder.Entity<Casting>().Property(c => c.Job).HasColumnName("job");
        modelBuilder.Entity<Casting>().Property(c => c.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<Casting>().HasKey(c => new { c.TitleId, c.PersonId, c.Ordering });// composite PK

        // map profession to Profession
        modelBuilder.Entity<Casting>().HasOne(c => c.Profession) 
                                      .WithMany()
                                      .HasForeignKey("profession_id");

        // map Genre to genre
        modelBuilder.Entity<Genre>().ToTable("genre");
        modelBuilder.Entity<Genre>().Property(g => g.Id).HasColumnName("genre_id");


        // map Person to person
        modelBuilder.Entity<Person>().ToTable("person");
        modelBuilder.Entity<Person>().Property(p => p.Id).HasColumnName("person_id");
        modelBuilder.Entity<Person>().Property(p => p.Name).HasColumnName("name");
        modelBuilder.Entity<Person>().Property(p => p.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(p => p.DeathYear).HasColumnName("death_year");

        // map Person to Casting
        modelBuilder.Entity<Person>()
                            .HasMany(p => p.Castings)
                            .WithOne(c => c.Person)
                            .HasForeignKey(c => c.PersonId);

        // map Person to PersonRating
        modelBuilder.Entity<Person>()
                            .HasOne(p => p.PersonRating)
                            .WithOne(pr => pr.Person)
                            .HasForeignKey<PersonRating>(pr => pr.PersonId);
    
        // map Person and KnownFor (Title) via table person_notable_title
        modelBuilder.Entity<Person>()
                    .HasMany(p => p.KnownFor)
                    .WithMany(t => t.KnownForPersons)
                    .UsingEntity<Dictionary<string, object>>(       // shadow entity for person_notable_title
                     "person_notable_title",                        // strings for table and FKs, because there's no class (nor navigation properties)
                     j => j.HasOne<Title>()
                     .WithMany()
                     .HasForeignKey("title_id"),
                     j => j.HasOne<Person>()
                     .WithMany()
                    .HasForeignKey("person_id"));



        // map Person to Profession. I haven't created a class for the join table(person_profession) since it only contains the two FKs
        modelBuilder.Entity<Person>().HasMany(p => p.Professions)
                            .WithMany()
                            .UsingEntity(j => j.ToTable("person_profession")
                            .HasData());
        
        // map Profession to profession
        modelBuilder.Entity<Profession>().ToTable("profession");
        modelBuilder.Entity<Profession>().Property(pr => pr.Id).HasColumnName("profession_id");


        //map Title to title
        modelBuilder.Entity<Title>().ToTable("title");
        modelBuilder.Entity<Title>().Property(t => t.Id).HasColumnName("title_id");
        modelBuilder.Entity<Title>().Property(t => t.PrimaryTitle).HasColumnName("primary_title");
        modelBuilder.Entity<Title>().Property(t => t.OriginalTitle).HasColumnName("original_title");
        modelBuilder.Entity<Title>().Property(t => t.IsAdult).HasColumnName("is_adult");
        modelBuilder.Entity<Title>().Property(t => t.StartYear).HasColumnName("start_year");
        modelBuilder.Entity<Title>().Property(t => t.EndYear).HasColumnName("end_year");
        modelBuilder.Entity<Title>().Property(t => t.RuntimeMinutes).HasColumnName("runtime_minutes");
        modelBuilder.Entity<Title>().Property(t => t.Plot).HasColumnName("plot");
        modelBuilder.Entity<Title>().Property(t => t.Poster).HasColumnName("poster");
        modelBuilder.Entity<Title>().Property(t => t.TypeId).HasColumnName("title_type_id");


        // map Title to Casting
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Cast)
                            .WithOne(c => c.Title)
                            .HasForeignKey(c => c.TitleId);


        // map Title and Directors (Person) via table title_director
        modelBuilder.Entity<Title>()
                    .HasMany(t => t.Directors)
                    .WithMany()
                    .UsingEntity<Dictionary<string, object>>(
                     "title_director",
                     j => j.HasOne<Person>()
                     .WithMany()
                     .HasForeignKey("person_id"),
                     j => j.HasOne<Title>()
                     .WithMany()
                    .HasForeignKey("title_id"));



        // map Title and Writers (Person) via table title_writer
        modelBuilder.Entity<Title>()
                    .HasMany(t => t.Writers)
                    .WithMany()
                    .UsingEntity<Dictionary<string, object>>(
                     "title_writer",
                     j => j.HasOne<Person>()
                     .WithMany()
                     .HasForeignKey("person_id"),
                     j => j.HasOne<Title>()
                     .WithMany()
                    .HasForeignKey("title_id"));


        // map Title and Genre via join table title_genre
        modelBuilder.Entity<Title>()
                    .HasMany(t => t.Genres)
                    .WithMany(g => g.Titles)
                    .UsingEntity<Dictionary<string, object>>(
                     "title_genre",
                     j => j.HasOne<Genre>()
                     .WithMany()
                     .HasForeignKey("genre_id"),
                     j => j.HasOne<Title>()
                     .WithMany()
                    .HasForeignKey("title_id"));


        // map Title to TitleType
        modelBuilder.Entity<Title>()
                            .HasOne(t => t.Type)
                            .WithMany(tt => tt.Titles)
                            .HasForeignKey(t => t.TypeId);

        // map Title to TitleRating
        modelBuilder.Entity<Title>()
                            .HasOne(t => t.TitleRating)
                            .WithOne(tr => tr.Title)
                            .HasForeignKey<TitleRating>(tr => tr.TitleId);

        //map TitleType to title_type
        modelBuilder.Entity<TitleType>().ToTable("title_type");
        modelBuilder.Entity<TitleType>().Property(tt => tt.Id).HasColumnName("title_type_id");


        //map Title to TitleAka
        modelBuilder.Entity<Title>()
                            .HasMany(t => t.Akas)
                            .WithOne()
                            .HasForeignKey("title_id");



        // map TitleAka to title_akas
        modelBuilder.Entity<TitleAka>().ToTable("title_aka");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Ordering).HasColumnName("ordering");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Region).HasColumnName("region");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Language).HasColumnName("language");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.Description).HasColumnName("description");
        modelBuilder.Entity<TitleAka>().Property(ta => ta.IsOriginalTitle).HasColumnName("is_original_title");
        modelBuilder.Entity<TitleAka>().HasKey(ta => new { ta.TitleId, ta.Ordering });// composite PK

        // map TitleAka to Context
        modelBuilder.Entity<TitleAka>()
                            .HasOne(ta => ta.Context)
                            .WithMany()
                            .HasForeignKey("context");



        // map Episode to episode
        modelBuilder.Entity<Episode>().ToTable("title_episode");
        modelBuilder.Entity<Episode>().Property(e => e.EpisodeId).HasColumnName("title_id_episode");
        modelBuilder.Entity<Episode>().Property(e => e.SeasonNumber).HasColumnName("seasonnumber");
        modelBuilder.Entity<Episode>().Property(e => e.EpisodeNumber).HasColumnName("episodenumber");

        // map Episode to Title
        modelBuilder.Entity<Episode>()
                            .HasOne(e => e.Title)
                            .WithMany()
                            .HasForeignKey("title_id_parent");

        // map Context to context
        modelBuilder.Entity<Context>().ToTable("context");
        modelBuilder.Entity<Context>().Property(c => c.Id).HasColumnName("context_id");


        // map TitleRating to title_rating
        modelBuilder.Entity<TitleRating>().ToTable("title_rating");
        modelBuilder.Entity<TitleRating>().Property(tr => tr.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<TitleRating>().Property(tr => tr.AverageRating).HasColumnName("average_rating");
        modelBuilder.Entity<TitleRating>().Property(tr => tr.Votes).HasColumnName("votes");
        modelBuilder.Entity<TitleRating>().HasKey(tr => tr.TitleId);

        // map PersonRating to person_title_rating
        modelBuilder.Entity<PersonRating>().ToTable("person_title_rating");
        modelBuilder.Entity<PersonRating>().Property(pr => pr.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<PersonRating>().Property(pr => pr.AverageRating).HasColumnName("average_rating");
        modelBuilder.Entity<PersonRating>().Property(pr => pr.Votes).HasColumnName("votes");
        modelBuilder.Entity<PersonRating>().HasKey(pr => pr.PersonId);

        // map TitleReadDto to output best_match_variadic()
        modelBuilder.Entity<TitleReadDto>().HasNoKey();
        //modelBuilder.Entity<TitleReadDto>().Property(trd => trd.TitleId).HasColumnName("match_title_id");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.Id).HasColumnName("title_id");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.PrimaryTitle).HasColumnName("primary_title");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.OriginalTitle).HasColumnName("original_title");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.IsAdult).HasColumnName("is_adult");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.StartYear).HasColumnName("start_year");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.EndYear).HasColumnName("end_year");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.RuntimeMinutes).HasColumnName("runtime_minutes");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.Plot).HasColumnName("plot");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.Poster).HasColumnName("poster");
        modelBuilder.Entity<TitleReadDto>().Property(t => t.TypeId).HasColumnName("title_type_id");


        /////////////////////////////////////////////////////////
        ///                        USER                       ///                   
        /////////////////////////////////////////////////////////

        //map BookmarkPerson to person_bookmark
        modelBuilder.Entity<BookmarkPerson>().ToTable("person_bookmark");
        modelBuilder.Entity<BookmarkPerson>().Property(bp => bp.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<BookmarkPerson>().Property(bp => bp.Username).HasColumnName("username");
        modelBuilder.Entity<BookmarkPerson>().Property(bp => bp.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<BookmarkPerson>().Property(bp => bp.Note).HasColumnName("note");
        modelBuilder.Entity<BookmarkPerson>().HasKey(bp => new { bp.PersonId, bp.Username });// composite PK

        // map PersonBookmark to Person and User
        modelBuilder.Entity<BookmarkPerson>()
                            .HasOne(bp => bp.Person)
                            .WithMany()
                            .HasForeignKey(bp => bp.PersonId);

        modelBuilder.Entity<BookmarkPerson>()
                            .HasOne(bp => bp.User)
                            .WithMany(u => u.BookmarkedPersons)
                            .HasForeignKey(bp => bp.Username);



        //map BookmarkTitle to title_bookmark
        modelBuilder.Entity<BookmarkTitle>().ToTable("title_bookmark");
        modelBuilder.Entity<BookmarkTitle>().Property(bt => bt.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<BookmarkTitle>().Property(bt => bt.Username).HasColumnName("username");
        modelBuilder.Entity<BookmarkTitle>().Property(bt => bt.CreatedAt).HasColumnName("created_at");
        modelBuilder.Entity<BookmarkTitle>().Property(bt => bt.Note).HasColumnName("note");
        modelBuilder.Entity<BookmarkTitle>().HasKey(bp => new { bp.TitleId, bp.Username });// composite PK

        // map PersonBookmark to Person and User
        modelBuilder.Entity<BookmarkTitle>()
                            .HasOne(bt => bt.Title)
                            .WithMany()
                            .HasForeignKey(bt => bt.TitleId);

        modelBuilder.Entity<BookmarkTitle>()
                            .HasOne(bt => bt.User)
                            .WithMany(u =>u.BookmarkedTitles)
                            .HasForeignKey(bt => bt.Username);

        // map Search to search_history
        modelBuilder.Entity<Search>().ToTable("search_history");
        modelBuilder.Entity<Search>().Property(s => s.Id).HasColumnName("search_id");
        modelBuilder.Entity<Search>().Property(s => s.SearchTime).HasColumnName("timestamp");
        modelBuilder.Entity<Search>().Property(s => s.Query).HasColumnName("query");

        // map User to user
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<User>().Property(u => u.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(u => u.FirstName).HasColumnName("first_name");
        modelBuilder.Entity<User>().Property(u => u.LastName).HasColumnName("last_name");
        modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");
        modelBuilder.Entity<User>().Property(u => u.Salt).HasColumnName("salt");
        modelBuilder.Entity<User>().HasKey(u => u.Username); // define PK


        // map User with Search
        modelBuilder.Entity<User>()
                            .HasMany(u => u.SearchHistory)
                            .WithOne(s => s.User)
                            .HasForeignKey("username");

        // map Rating to rating_history
        modelBuilder.Entity<Rating>().ToTable("rating_history");
        modelBuilder.Entity<Rating>().Property(r => r.Username).HasColumnName("username");
        modelBuilder.Entity<Rating>().Property(r => r.TitleId).HasColumnName("title_id");
        modelBuilder.Entity<Rating>().Property(r => r.Id).HasColumnName("rating_id");
        modelBuilder.Entity<Rating>().Property(r => r.RatingValue).HasColumnName("rating");
        modelBuilder.Entity<Rating>().Property(r => r.RatingDate).HasColumnName("rate_time");


        // map Rating to User
        modelBuilder.Entity<Rating>()
                            .HasOne(r => r.User)
                            .WithMany(u => u.RatedTitles)
                            .HasForeignKey(r => r.Username);

        // map Rating to Title
        modelBuilder.Entity<Rating>()
                            .HasOne(r => r.Title)
                            .WithMany()
                            .HasForeignKey(r => r.TitleId);
    }
}
