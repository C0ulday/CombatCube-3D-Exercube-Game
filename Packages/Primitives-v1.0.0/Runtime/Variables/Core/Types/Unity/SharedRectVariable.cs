namespace Koboldgames.Primitives.Variables
{
    using System;
    using UnityEngine;

    public abstract class SharedRectVariable<T> : SharedVariableBase<T>, IRectVariable
    {
        public Rect RectValue
        {
            get
            {
                Rect? direct = value as Rect?;

                if(direct != null)
                    return direct.Value;

                RectInt? convert = value as RectInt?;

                if(convert != null)
                    return new Rect(convert.Value.position, convert.Value.size);

                throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Rect'!");
            }
        }

        public RectInt RectIntValue
        {
            get
            {
                RectInt? direct = value as RectInt?;

                if(direct != null)
                    return direct.Value;

                Rect? convert = value as Rect?;

                if(convert != null)
                {
                    return new RectInt(
                        (int)convert.Value.x,
                        (int)convert.Value.y,
                        (int)convert.Value.width,
                        (int)convert.Value.height
                    );
                }

                throw new InvalidCastException($"Can not cast type '{typeof(T).Name}' to 'Rect'!");
            }
        }
    }
}
