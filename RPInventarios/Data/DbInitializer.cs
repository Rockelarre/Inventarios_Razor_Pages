using RPInventarios.Models;

namespace RPInventarios.Data;

    public static class DbInitializer
    {
        public static void Initialize(InventariosContext context)
        {
            // Checamos si existe alguna Marca
            if (context.Marca.Any())
            {
                return; //La BD ha sido inicializada con información
            }

            var marcas = new Marca[]
            {
                    new Marca{Nombre="Rino"},
                    new Marca{Nombre="Rocco"},
                    new Marca{Nombre="Azuri"},
                    new Marca{Nombre="Reni"},
                    new Marca{Nombre="Bazi"},
                    new Marca{Nombre="Asis"}
            };
            context.Marca.AddRange(marcas);
            context.SaveChanges();

            var departamentos = new Departamento[]
            {
                        new Departamento{
                            Nombre="Administración General",
                            Descripcion="Administración General",
                            FechaCreacion=DateTime.Now
                        },
                        new Departamento{
                            Nombre="Recursos Humanos",
                            Descripcion="Recursos Humanos",
                            FechaCreacion=DateTime.Now
                        },
                        new Departamento{
                            Nombre="Recursos Materiales",
                            Descripcion="Recursos Materiales",
                            FechaCreacion=DateTime.Now
                        },
                        new Departamento{
                            Nombre="Informática",
                            Descripcion="Informática",
                            FechaCreacion=DateTime.Now
                        },
                        new Departamento{
                            Nombre="Deportes",
                            Descripcion="Deportes",
                            FechaCreacion=DateTime.Now
                        }
            };
            context.Departamento.AddRange(departamentos);
            context.SaveChanges();

            var productos = new Producto[]
            {
                    new Producto
                    {
                        Nombre="Silla Secretarial",
                        Descripcion="Silla de imitación piel",
                        MarcaId = context.Marca.First(u=>u.Nombre=="Rino").Id,
                        Costo=2500M
                    },
                    new Producto
                    {
                        Nombre="Escritorio Gerencial",
                        Descripcion="Escritorio negro con cristal templado",
                        MarcaId = context.Marca.First(u=>u.Nombre=="Azuri").Id,
                        Costo=2500M
                    },
                    new Producto
                    {
                        Nombre="Cafetera Industrial",
                        Descripcion="Cafeteria para 50 tazas",
                        MarcaId = context.Marca.First(u=>u.Nombre=="Rocco").Id,
                        Costo=2500M
                    },
                    new Producto
                    {
                        Nombre="Computadora",
                        Descripcion="Computadora Gamer",
                        MarcaId = context.Marca.First(u=>u.Nombre=="Asis").Id,
                        Costo=65500M
                    },
                    new Producto
                    {
                        Nombre="Proyector",
                        Descripcion="Proyector Inalámbrico",
                        MarcaId = context.Marca.First(u=>u.Nombre=="Reni").Id,
                        Costo=6500M
                    },
            };
            context.Producto.AddRange(productos);
            context.SaveChanges();
    }
    }

