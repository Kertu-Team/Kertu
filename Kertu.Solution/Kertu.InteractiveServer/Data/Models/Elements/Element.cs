﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kertu.InteractiveServer.Data.Models.Elements
{
    /// <summary>
    /// Base class for future elements
    /// </summary>
    /// <remarks>
    /// All elements such as lists or cards should inherit this class
    /// </remarks>
    public abstract class Element
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// DO NOT USE
        /// </summary>
        [ForeignKey(nameof(ApplicationUser))]
        [Column(TypeName = "varchar(255)")]
        public string? ApplicationUserId { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string? OwnerId { get; set; }

        public virtual ApplicationUser? Owner { get; set; }

        public int? ParentID { get; set; }
        public int? ListID { get; set; }
        public int? BoardID { get; set; }

        public int Position { get; set; }

        /// <summary>
        /// Adds this element as a child of another element. Ensure that the parent element exists in the database, because it needs to have ID
        /// </summary>
        /// <param name="element">Parent element</param>
        /// <exception cref="InvalidOperationException">Cards can not have children, sorry</exception>
        public void AddTo(Element element)
        {
            if (element is Card card)
            {
                throw new InvalidOperationException("Cards can not have children, sorry");
            }
            else if (element is List list)
            {
                if(element.GetChildren().Count > 0)
                    this.Position = element.GetChildren().OrderBy(e => e.Position).Last().Position + 1;
                list.Children.Add(this as Card);
                this.ParentID = list.Id;
            }
            else if (element is Board board)
            {
                if (this is Card c)
                {
                    if (element.GetChildren().Count > 0)
                        this.Position = element.GetChildren().OrderBy(e => e.Position).Last().Position + 1;
                    board.Children.Add(c);
                    this.ParentID = board.Id;
                }
                else if (this is List l)
                {
                    if (element.GetChildren().Count > 0)
                        this.Position = element.GetChildren().OrderBy(e => e.Position).Last().Position + 1;
                    board.Children.Add(l);
                    this.ParentID = board.Id;
                }
                else if (this is Board b)
                {
                    if (element.GetChildren().Count > 0)
                        this.Position = element.GetChildren().OrderBy(e => e.Position).Last().Position + 1;
                    board.Children.Add(b);
                    this.ParentID = board.Id;
                }
            }
        }

        /// <summary>
        /// Returns children if it has any
        /// </summary>
        /// <returns></returns>
        public List<Element> GetChildren()
        {
            if (this is List list)
            {
                return list.Children.Cast<Element>().ToList();
            }
            else if (this is Board board)
            {
                return board.Children.Cast<Element>().ToList();
            }
            else
                return new List<Element>();
        }
    }
}
