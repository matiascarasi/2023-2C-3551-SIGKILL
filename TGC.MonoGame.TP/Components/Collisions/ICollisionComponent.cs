namespace TGC.MonoGame.TP.Components.Collisions
{
    interface ICollisionComponent
    {
        public void LoadContent(GameObject gameObject);
        public void Update(GameObject gameObject);
        public void Draw(Gizmos.Gizmos gizmos);
    }
}
