using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PapaMarti.PizzaMaking {
    class ToppingList {

        List<Point> toppings;
        List<Point> usedPoints;

        public Topping type {
            get; private set;
        }

        public ToppingList(Topping type, List<Point> points) {
            this.type = type;
            this.toppings = points;
            usedPoints = new List<Point>();
        }

        public bool hasAll() {
            return toppings.Count == 0;
        }

        public void register(Point point) {
            
        }

        public Point findNearest(Point other) {
            double smallest = double.MaxValue;
            int index = -1;

            for(int i = 0; i < toppings.Count; i++) {
                double distance = Math.Sqrt(Math.Pow(toppings.ElementAt(index).X + other.X, 2) + Math.Pow(toppings.ElementAt(index).Y + other.Y, 2));
                if(distance < smallest) {
                    smallest = distance;
                    index = i;
                }
            }

            return index == -1 ? new Point(-1, -1) : toppings.ElementAt(index);
        }

    }
}
