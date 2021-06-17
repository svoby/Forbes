namespace Forbes.Inputs
{
    [System.Serializable]
    public class InputFrame
    {
        public float Horizontal;
        public float Vertical;
        public float Rotation;
        public bool Jump;
        public int Fire;
        public InputMask Mask;
        public float Timestamp;

        // public override bool Equals(object obj)
        // {
        //     return obj is InputFrame frame &&
        //                  this.Horizontal == frame.Horizontal &&
        //                  this.Vertical == frame.Vertical &&
        //                  this.Rotation == frame.Rotation &&
        //                  this.Jump == frame.Jump &&
        //                  this.Fire == frame.Fire;
        // }
    }
}