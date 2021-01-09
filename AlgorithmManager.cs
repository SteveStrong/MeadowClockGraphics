using Meadow.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeadowClockGraphics
{

    public enum Operation
    {
        ON,
        OFF,
        RUN,
        STOP,
        SHOW,
        HIDE,
        EXECUTE
    }

    public class LightLED
    {
        public int Id { get; set; }
        public int groupId { get; set; }
        public string name { get; set; }
        public Color color { get; set; }

        public LightLED(int Id, int groupId, Color color)
        {
            this.Id = Id;
            this.groupId = groupId;
            this.color = color;
        }
    }

    public class Instruction
    {
        public Operation op { get; set; }
        public LightLED led { get; set; }

        public Instruction(Operation op, LightLED led)
        {
            this.op = op;
            this.led = led;
        }
    }


    public class Algorithm
    {
        private readonly Dictionary<int, List<Instruction>> steps = new Dictionary<int, List<Instruction>>();

        public Algorithm addStep(int id, Instruction obj) {

            if ( !steps.ContainsKey(id) ) {
                steps[id] = new List<Instruction>();
            }
            steps[id].Add(obj);
            return this;
        }

        public List<Instruction> getStep(int id) {
            return steps[id];
        }
    }

    public class AlgorithmManager
    {
        Algorithm program = new Algorithm();

        public Algorithm addStep(int id, Instruction obj) {
            return program.addStep(id, obj);
        }

        public List<Instruction> getStep(int id) {
            return program.getStep(id);
        }
    }
    
}
