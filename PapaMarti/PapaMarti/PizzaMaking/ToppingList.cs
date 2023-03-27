namespace PapaMarti {
    public class ToppingList {

        public int desired {
            get; private set;
        }

        public int has {
            get; set;
        }

        public Topping type {
            get; private set;
        }

        public ToppingList(Topping type, int desired) {
            this.type = type;
            this.desired = desired;
            has = 0;
        }

        public bool hasAll() {
            return has == desired;
        }

    }
}
