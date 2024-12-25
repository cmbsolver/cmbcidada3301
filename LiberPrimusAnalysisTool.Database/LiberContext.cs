using LiberPrimusAnalysisTool.Entity.Text;
using Microsoft.EntityFrameworkCore;

namespace LiberPrimusAnalysisTool.Database
{
    /// <summary>
    /// LiberContext
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class LiberContext : DbContext
    {
        private readonly string _connectionString = "<connection string>";
        
        public LiberContext()
        {
            FileInfo fileInfo = new(Environment.ProcessPath);
            _connectionString = File.ReadAllText($"{fileInfo.Directory}/connstring.txt");
        }
        
        public DbSet<TextDocument> TextDocuments { get; set; }
        
        public DbSet<TextDocumentCharacter> TextDocumentCharacters { get; set; }
        
        public DbSet<LiberTextDocumentCharacter> LiberTextDocumentCharacters { get; set; }
        
        public DbSet<RuneTextDocumentCharacter> RuneTextDocumentCharacters { get; set; }
        
        public DbSet<DictionaryWord> DictionaryWords { get; set; }

        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        /// <remarks>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </para>
        /// </remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_connectionString);

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// <para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run. However, it will still run when creating a compiled model.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
        /// examples.
        /// </para>
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TextDocument>()
                .ToTable("TB_FILE")
                .Property(b => b.Id)
                .UseIdentityAlwaysColumn();
            
            modelBuilder.Entity<TextDocumentCharacter>()
                .ToTable("TB_FILE_CHARACTER_COUNT")
                .Property(b => b.Id)
                .UseIdentityAlwaysColumn();
            
            modelBuilder.Entity<LiberTextDocumentCharacter>()
                .ToTable("TB_LIBER_FILE_CHARACTER_COUNT")
                .Property(b => b.Id)
                .UseIdentityAlwaysColumn();
            
            modelBuilder.Entity<RuneTextDocumentCharacter>()
                .ToTable("TB_RUNE_FILE_CHARACTER_COUNT")
                .Property(b => b.Id)
                .UseIdentityAlwaysColumn();
            
            modelBuilder.Entity<DictionaryWord>()
                .ToTable("TB_DICT_WORD")
                .Property(b => b.Id)
                .UseIdentityAlwaysColumn();
        }
    }
}