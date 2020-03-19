using FluentNHibernate.Mapping;

namespace WebWIthNHibernate.Models
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Id(p => p.Id).GeneratedBy.Identity();
            Map(p => p.Nome).Length(50);
            Map(p => p.Idade);
            Table("Pessoas");
        }
    }
}
