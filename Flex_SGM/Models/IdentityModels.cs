using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Flex_SGM.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }

        [MaxLength(150)]
        public string UserFullName { get; set; }

        [MaxLength(50)]
        public string Area { get; set; }

        [MaxLength(50)]
        public string Puesto { get; set; }

        [MaxLength(20)]
        public string Nomina { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Maquinas> Maquinas { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Bitacora> Bitacoras { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.OILs> OILs { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Fallas> Fallas { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Junta> Juntas { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Proyectos> Proyectos { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Comments> Comments { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.CalendarioProye> CalendarioProye { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.controlplanos> controlplanos { get; set; }
        public System.Data.Entity.DbSet<Flex_SGM.Models.CReclamos> reo { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.ControldeEquipos> ControldeEquipos { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.CAndon> CAndons { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.CDockaudit> CDockaudits { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.Disponibilidad> Disponibilidads { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.troubleshooting> troubleshootings { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.cClientes> cClientes { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.SubClientes> SubClientes { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.AndonDefecto> AndonDefectoes { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.cAreas> cAreas { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.AndonSupervisores> AndonSupervisores { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.AndonAuditor> AndonAuditors { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.CAndon2> CAndon2 { get; set; }

        public System.Data.Entity.DbSet<Flex_SGM.Models.cProyectos> cProyectos { get; set; }
    }
}