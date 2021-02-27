using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Unity.PerformanceTesting;

namespace Game.Net.Tests
{
    public class BufferEntityTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BufferEntityTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BufferEntityTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }

    public class BufferEntityEncodeTests
    {
        [TestCase(1, 2, 3, 4, 5, new byte[] { 0 })]
        public void TestEncode(int seesion, int sId, int moudleId, int messageType, int messageId, byte[] protocal) 
        {
            BufferEntity entity = new BufferEntity(new System.Net.IPEndPoint(1, 1), seesion, sId, moudleId, messageType, messageId, protocal);

            Assert.AreEqual(entity.Encode(true), entity.Encoder(true));
        }

        [TestCase(1, 2, 3, 4, 5, new byte[] { 0 }), Performance]
        public void TestEncodePerformance(int seesion, int sId, int moudleId, int messageType, int messageId, byte[] protocal)
        {
            BufferEntity entity = new BufferEntity(new System.Net.IPEndPoint(1, 1), seesion, sId, moudleId, messageType, messageId, protocal);
            Measure.Method(() => { entity.Encode(false); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }

        [TestCase(1, 2, 3, 4, 5, new byte[] { 0 }), Performance]
        public void TestEncode2Performance(int seesion, int sId, int moudleId, int messageType, int messageId, byte[] protocal)
        {
            BufferEntity entity = new BufferEntity(new System.Net.IPEndPoint(1, 1), seesion, sId, moudleId, messageType, messageId, protocal);
            Measure.Method(() => { entity.Encoder(false); })
                .WarmupCount(10)
                .MeasurementCount(10)
                .IterationsPerMeasurement(5)
                .GC()
                .Run();
        }
    }
}
