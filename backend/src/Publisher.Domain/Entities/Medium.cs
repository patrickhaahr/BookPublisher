namespace Publisher.Domain.Entities;

public enum Medium
{
    // Physical formats
    Print = 1, // Physical book
    Magazine = 2, // Magazine
    
    // Digital formats
    EBook = 3, // Digital book
    AudioBook = 4, // Audio book
    
    // Novel types
    Novel = 5, // Novel
    LightNovel = 6, // Light novel
    WebNovel = 7, // Web novel
    
    // Comic/Graphic formats
    GraphicNovel = 8, // Graphic novel
    Comic = 9, // Comic
    Manga = 10, // Manga - Japanese comics
    Manhwa = 11, // Manhwa - Korean comics
    Manhua = 12, // Manhua - Chinese comics
}
