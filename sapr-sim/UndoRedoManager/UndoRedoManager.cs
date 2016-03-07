using Entities;
using sapr_sim.Figures;
using sapr_sim.Utils;
using sapr_sim.WPFCustomElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim
{
    public class UndoRedoManager
    {
        private static Dictionary<long, FixedSizedStack<byte[]>> undoStack = new Dictionary<long, FixedSizedStack<byte[]>>();
        private static Dictionary<long, FixedSizedStack<byte[]>> redoStack = new Dictionary<long, FixedSizedStack<byte[]>>();



        public static ScrollableCanvas proceed(long canvasId, Dictionary<long, FixedSizedStack<byte[]>> stack)
        {
            if (stack.ContainsKey(canvasId))
            {
                FixedSizedStack<byte[]> stck = stack[canvasId];
                if (stck.HasElements())
                {
                    byte[] canvas = stck.Pop();
                    Stream stream = StreamUtils.readFromByteArray(canvas);

                    return (ScrollableCanvas)new BinaryFormatter().Deserialize(stream);
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        private static void putInStack(ScrollableCanvas scrollableCanvas, Dictionary<long, FixedSizedStack<byte[]>> stack)
        {
            long id = scrollableCanvas.id;

            Stream stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream, scrollableCanvas);

            if (stack.ContainsKey(id))
            {
                FixedSizedStack<byte[]> fsq = stack[id];
                fsq.Push(StreamUtils.ReadFromStream(stream));
            }
            else
            {
                FixedSizedStack<byte[]> fsq = new FixedSizedStack<byte[]>();
                fsq.Push(StreamUtils.ReadFromStream(stream));
                stack.Add(id, fsq);
            }
        }

        public static ScrollableCanvas undoProceed(long canvasId)
        {
            return proceed(canvasId, undoStack);
        }

        public static ScrollableCanvas redoProceed(long canvasId)
        {
            return proceed(canvasId, redoStack);
        }

        public static void putInUndoStack(UIEntity selected)
        {
            ScrollableCanvas canvas = (ScrollableCanvas)selected.canvas;
            putInUndoStack(canvas);
        }

        public static void putInUndoStack(ScrollableCanvas scrollableCanvas)
        {
            putInStack(scrollableCanvas, undoStack);
        }

        public static void putInRedoStack(UIEntity selected)
        {
            ScrollableCanvas canvas = (ScrollableCanvas)selected.canvas;
            putInRedoStack(canvas);
        }

        public static void putInRedoStack(ScrollableCanvas scrollableCanvas)
        {
            putInStack(scrollableCanvas, redoStack);
        }


        public static void clearRedoStack(UIEntity selected)
        {
            ScrollableCanvas canvas = (ScrollableCanvas)selected.canvas;
            clearRedoStack(canvas);
        }

        public static void clearRedoStack(ScrollableCanvas scrollableCanvas)
        {
            long id = scrollableCanvas.id;

            if (redoStack.ContainsKey(id))
            {
                redoStack[id].Clear() ;
            }
        }

    }
}
