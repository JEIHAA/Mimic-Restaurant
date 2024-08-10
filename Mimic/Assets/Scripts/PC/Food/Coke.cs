public class Coke : FoodInfo
{
    private void Start()
    {
        fillHunger = foodstat.hungerrestore;
        price = foodstat.money; 
    }
}
