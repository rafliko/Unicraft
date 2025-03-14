public enum BlockType
{
    Air, Grass, Dirt, Stone, Test, Bricks
}

public enum Face
{
    Top,
    Bottom,
    Left,
    Right,
    Front,
    Back
}

public struct ChunkPos
{
    public int x, z;
    public ChunkPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
