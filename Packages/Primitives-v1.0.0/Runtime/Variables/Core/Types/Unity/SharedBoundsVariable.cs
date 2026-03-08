namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    public abstract class SharedBoundsVariable<T> : SharedVariableBase<T>, IBoundsVariable
    {
        public Bounds BoundsValue
        {
            get
            {
                Bounds? direct = value as Bounds?;

                if(direct != null)
                    return direct.Value;

                BoundsInt? convert = value as BoundsInt?;

                if(convert != null)
                    return new Bounds(convert.Value.center, convert.Value.size);

                throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Bounds'!");
            }
        }

        public BoundsInt BoundsIntValue
        {
            get
            {
                BoundsInt? direct = value as BoundsInt?;

                if(direct != null)
                    return direct.Value;

                Bounds? convert = value as Bounds?;

                if(convert != null)
                {
                    return new BoundsInt(
                        (int)convert.Value.min.x,
                        (int)convert.Value.min.y,
                        (int)convert.Value.min.z,
                        (int)convert.Value.size.x,
                        (int)convert.Value.size.y,
                        (int)convert.Value.size.z
                    );
                }

                throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Bounds'!");
            }
        }
    }
}
