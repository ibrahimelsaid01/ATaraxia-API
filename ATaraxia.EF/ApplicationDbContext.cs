namespace ATaraxia.EF;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Device>()
            .HasKey(d => d.DeviceId);
        builder.Entity<Device>()
            .HasOne<User>(e => e.Users)
            .WithMany(g => g.DeviceIdList)
            .HasForeignKey(e => e.UserId);

        builder.Entity<Question>().HasKey(Q => Q.QuestionId);
        builder.Entity<Question>().Property(Q => Q.Ask);
        builder.Entity<Question>().Property(Q => Q.Answer);
        builder.Entity<Question>()
           .HasOne<User>(e => e.Users)
           .WithMany(g => g.Recomendation)
           .HasForeignKey(e => e.UserId);

        builder.Entity<User>().HasKey(U => U.UserId);
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().Property(U => U.Gender);
        builder.Entity<User>().Property(U => U.LoginStatus);
        builder.Entity<User>().Property(U => U.NickName);


        builder.Entity<UserLike>().HasKey(L => L.UserlikeId);
        builder.Entity<UserLike>()
          .HasOne<Template>(e => e.Template)
          .WithMany(g => g.UserLikes)
          .HasForeignKey(e => e.TemplateId);
        base.OnModelCreating(builder);

    }
    public virtual DbSet<Template>? Temblates { get; set; }
    public virtual DbSet<User>? CustomUsers { get; set; }
    public virtual DbSet<Device>? Devices { get; set; }
    public virtual DbSet<Question>? Questions { get; set; }
    public virtual DbSet<UserLike>? UserLikes { get; set; }
}