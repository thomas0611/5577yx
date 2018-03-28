using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Model
{
    public class News
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int GameId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public DateTime ReleaseTime { get; set; }
        public string Photo { get; set; }
        public string Source { get; set; }
        public string NewsContent { get; set; }
        public int SortId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyWord { get; set; }
        public string SeoDesc { get; set; }
        public int IsTop { get; set; }
        public int IsRed { get; set; }
        public int IsHot { get; set; }
        public int IsSlide { get; set; }
        public int IsLock { get; set; }
        public string NameColor { get; set; }

        public News()
        {

        }
        public News(int Id, int TypeId, int GameId, int Type, string Title, string KeyWord, DateTime ReleaseTime, string Photo, string Source,
                    string NewsContent, int SortId, string SeoTitle, string SeoKeyWord, string SeoDesc, int IsTop, int IsRed, int IsHot,
                    int IsSlide, int IsLock, string NameColor)
        {
            this.Id = Id;
            this.TypeId = TypeId;
            this.GameId = GameId;
            this.Type = Type;
            this.Title = Title;
            this.KeyWord = KeyWord;
            this.ReleaseTime = ReleaseTime;
            this.Photo = Photo;
            this.Source = Source;
            this.NewsContent = NewsContent;
            this.SortId = SortId;
            this.SeoTitle = SeoTitle;
            this.SeoKeyWord = SeoKeyWord;
            this.SeoDesc = SeoDesc;
            this.IsTop = IsTop;
            this.IsRed = IsRed;
            this.IsHot = IsHot;
            this.IsSlide = IsSlide;
            this.IsLock = IsLock;
            this.NameColor = NameColor;

        }
    }
}
