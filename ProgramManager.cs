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
        int Id { get; set; }
        int groupId { get; set; }
        string name { get; set; }
        Color color { get; set; }

        LightLED(int Id, int groupId, Color color)
        {
            this.Id = Id;
            this.groupId = groupId;
            this.color = color;
        }
    }

    public class Instruction
    {
        Operation op { get; set; }
        LightLED data { get; set; }

        Instruction(Operation op, LightLED data)
        {
            this.op = op;
            this.data = data;
        }
    }


    public class Algorithm
    {
        private readonly Dictionary<int, List<Instruction>> steps = new Dictionary<int, List<Instruction>>();

        public Algorithm addStep(int id, Instruction obj) {

            if ( steps.ContainsKey(id) ) {
                steps[id] = new List<Instruction>();
            }
            steps[id].Add(obj);
            return this;
        }

        public List<Instruction> getStep(int id) {
            return steps[id];
        }
    }

    public class ProgramManager
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
