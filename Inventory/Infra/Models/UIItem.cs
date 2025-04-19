using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infra.Models
{
   public class UIItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string CategoryAndSubCategory { get; set; }

        public string CategoryName { get; set; }

        public Color CategoryColor { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public string SubCategoryIcon { get; set; }

        public int SituationId { get; set; }

        public string TechnicalDescription { get; set; }

        public string Comments { get; set; }
    }


    //public partial class ItemGroup : List<UIItem>
    //{
    //    public string GroupingParameter { get; set; }

    //    public ItemGroup(string orderParameter, List<UIItem> items) : base(items)
    //    {
    //        GroupingParameter = orderParameter;
    //    }
    //}

    //public class ItemList
    //{
    //public static List<UIItem> ListItems { get; } = new()
    //    {
    //        new UIItem {
    //            Id= 1,
    //            Name = "Mesa Retangular Para Sala De Jantar Casa D 120cm Veneza",
    //            CategoryAndSubCategory = "Casa>Móveis",
    //            CategoryColor = Color.FromArgb("#EF7C8E"),
    //            Status="Em uso",
    //            SubCategoryIcon="\uf4b8",CategoryId =1 ,StatusId=1},

    //        new UIItem {
    //            Id= 2,
    //            Name = "Fogão Atlas Mônaco Top Glass 4 Bocas Preto - Bivolt",
    //            CategoryAndSubCategory = "Casa>Eletrodomésticos",
    //            CategoryColor=Color.FromArgb("#FAE8E0"),
    //            Status="Em uso",
    //            SubCategoryIcon= "\uf26c",CategoryId =1,StatusId=1
    //        },
    //        new UIItem {
    //            Id= 3,
    //            Name = "Purificador de Água Eletrônico Philco PBE05CF Preto Bivolt",
    //            CategoryAndSubCategory = "Casa>Eletroportáteis",
    //            CategoryColor=Color.FromArgb("#EF7C8E"),
    //            Status="Emprestado",
    //            SubCategoryIcon= "\uf517",CategoryId =1,StatusId=2
    //        },
    //        new UIItem {
    //            Id= 4,
    //            Name = "Notebook Multilaser PC134",
    //            CategoryAndSubCategory = "Casa>Computadores",
    //            CategoryColor=Color.FromArgb("#EF7C8E"),
    //            Status="Emprestado",
    //            SubCategoryIcon = "\uf108",CategoryId =1,StatusId=2
    //        },

    //        new UIItem {
    //            Id= 5,
    //            Name = "Smart TV 50\" Crystal UHD 4K Samsung 50BU8000",
    //            CategoryAndSubCategory = "Casa>Eletrodomésticos",
    //            CategoryColor=Color.FromArgb("#76B947"),
    //            Status="Revendido",
    //            SubCategoryIcon= "\uf26c",CategoryId =1,StatusId=3
    //        },
    //        new UIItem {
    //            Id= 6,
    //            Name = "Samsung Smart TV 32\" LH32BETBLGGXZD LED 2 HDMI 1 USB",
    //            CategoryAndSubCategory = "Casa>Eletrodomésticos",
    //            CategoryColor = Color.FromArgb("#76B947"), Status="Revendido"
    //            ,SubCategoryIcon= "\uf26c",CategoryId =1,StatusId=3 },
    //        new UIItem {
    //            Id= 7,
    //            Name = "Smartphone Samsung Galaxy Note 20", CategoryAndSubCategory = "Vestimenta",CategoryId=4
    //            ,CategoryColor=Color.FromArgb("#B6E2D3"),Status="Em uso",SubCategoryIcon= "\uf10b",StatusId=1 },
    //        new UIItem {
    //            Id= 8,
    //            Name = "Smartphone Samsung Galaxy A52", CategoryAndSubCategory = "Vestimenta",CategoryId=4,
    //            CategoryColor=Color.FromArgb("#B6E2D3"),Status="Revendido",SubCategoryIcon= "\uf10b" , StatusId = 3},

    //        new UIItem {
    //            Id= 9,
    //            Name = "Colchão Casal Molas Ensacadas com Pillow Top Extra Conforto 138x188x38cm - Premium Sleep - bf Colchões",
    //            CategoryAndSubCategory = "Casa>Móveis",CategoryId=1,CategoryColor=Color.FromArgb("#EF7C8E"),Status="Em uso",SubCategoryIcon="\uf02b" , StatusId = 1},
    //        new UIItem {
    //            Id= 10,
    //            Name = "Ventilador de Mesa 30cm Mondial VSP-30-B Super Power Preto - 110v",CategoryId=1,
    //            CategoryAndSubCategory = "Casa>Eletroportáteis",CategoryColor=Color.FromArgb("#EF7C8E"),Status="Em uso",SubCategoryIcon="\uf02b",StatusId=1 },
    //        new UIItem {
    //            Id= 11,
    //            Name = "Skate Estampado - brink +", CategoryAndSubCategory = "Casa>Esporte",
    //            CategoryColor=Color.FromArgb("#D8A7B1"),
    //            Status="Guardado",
    //            SubCategoryIcon="\uf02b",
    //            CategoryId=1 , StatusId = 4},
    //        new UIItem {
    //            Id= 12,
    //            Name = "Bola Futebol Campo Oficial Topper Slick Amarelo",
    //            CategoryAndSubCategory = "Casa>Esporte",CategoryColor=Color.FromArgb("#D8A7B1"),
    //            Status="Emprestado",
    //            SubCategoryIcon="\uf02b",CategoryId=1,StatusId=2 }
    //};
    //}
}
