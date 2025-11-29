namespace Muchacho.Models
{
    public class Music
    {
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string CoverUrl { get; set; }
        public float Price { get; set; } = 9.99f;

        // Lista estática en memoria con álbumes a la venta
        public static List<Music> Musics = new List<Music>
        {
            new Music { Id = 1, Artist = "Title Fight", Album = "Shed", Year = 2011, CoverUrl="https://i.pinimg.com/1200x/89/da/9d/89da9d3fec18706e7cf59eebd28836af.jpg", Price=200 },
            new Music { Id = 2, Artist = "Title Fight", Album = "Floral Green", Year = 2012, CoverUrl = "https://i.pinimg.com/736x/5e/29/32/5e2932d87ddb362669d835c4fe3f3366.jpg", Price=205},
            new Music { Id = 3, Artist = "Title Fight", Album = "Hyperview", Year = 2015, CoverUrl= "https://i.pinimg.com/736x/12/f5/3c/12f53c9b4f10285f22860d13872726aa.jpg" , Price=150},

            new Music { Id = 4, Artist = "Fleshwater", Album = "We're Not Here to Be Loved", Year = 2022, CoverUrl= "https://i.pinimg.com/1200x/c4/7f/c2/c47fc2df85e676bb75d4b8d8bf2e77d3.jpg", Price=300 },
            new Music { Id = 5, Artist = "Fleshwater", Album = "Demo 2020", Year = 2020, CoverUrl = "https://f4.bcbits.com/img/a2772482028_10.jpg", Price=500 },


            new Music { Id = 11, Artist = "Rescate", Album = "Quitamancha", Year = 1997, CoverUrl= "https://www.cmtv.com.ar/tapas-cd/rescatequitamancha.webp", Price=90 },
            new Music { Id = 12, Artist = "Rescate", Album = "Buscando Lío", Year = 2000, CoverUrl = "https://www.cmtv.com.ar/tapas-cd/rescatebuscandolio.webp", Price=350 },
            new Music { Id = 13, Artist = "Rescate", Album = "Una Raza Contra el Viento", Year = 2004, CoverUrl= "https://www.cmtv.com.ar/tapas-cd/rescateunaraza.webp", Price=215 },

            new Music { Id = 14, Artist = "Roca Firme", Album = "Manos Heridas", Year = 2007, CoverUrl= "https://cdn-images.dzcdn.net/images/cover/6657c0e30cec2e2e5bcfe07a8a81d365/500x500-000000-80-0-0.jpg", Price=200},
            new Music { Id = 15, Artist = "Roca Firme", Album = "Volumen Uno", Year = 2018, CoverUrl = "https://cdn-images.dzcdn.net/images/cover/0c24b29a6e41cbe4527f517f43ef8b0b/500x500-000000-80-0-0.jpg", Price=125},

            new Music { Id = 16, Artist = "Mei Semones", Album = "Kabutomushi", Year = 2022, CoverUrl="https://cdn-images.dzcdn.net/images/cover/64b8502a43672ca8944f7a5732875e6e/500x500-000000-80-0-0.jpg", Price=205 },
            new Music { Id = 17, Artist = "Mei Semones", Album = "Tsukino", Year = 2023, CoverUrl= "https://cdn-images.dzcdn.net/images/cover/eccc31e880402aaf9fd7c8c687ce724c/500x500-000000-80-0-0.jpg" , Price=300},

            new Music { Id = 18, Artist = "Fermín IV", Album = "Boomerang", Year = 2002, CoverUrl="https://cdn-images.dzcdn.net/images/cover/8144470142b569e9032d298e582c6139/500x500-000000-80-0-0.jpg", Price=140 },
            new Music { Id = 19, Artist = "Fermín IV", Album = "Odio/Amor", Year = 2017, CoverUrl= "https://cdn-images.dzcdn.net/images/cover/125bcfc7661d4e09d9b4d494065f02f9/500x500-000000-80-0-0.jpg" , Price=200},

            new Music { Id = 20, Artist = "Control Machete", Album = "Mucho Barato...", Year = 1997, CoverUrl= "https://cdn-images.dzcdn.net/images/cover/b1b38559dd32039e3eef6691ab511546/500x500-000000-80-0-0.jpg", Price=145 },
            new Music { Id = 21, Artist = "Control Machete", Album = "Artillería Pesada Presenta", Year = 1999, CoverUrl="https://cdn-images.dzcdn.net/images/cover/f2de101cb94da796833120f67b0224f3/500x500-000000-80-0-0.jpg", Price=245},
            new Music { Id = 22, Artist = "Control Machete", Album = "Uno, Dos: Bandera", Year = 2003, CoverUrl="https://cdn-images.dzcdn.net/images/cover/01d1689086be91afc646c1b273a70e7a/500x500-000000-80-0-0.jpg" , Price=500},


        };
    }
}