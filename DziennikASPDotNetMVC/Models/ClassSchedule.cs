using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DziennikASPDotNetMVC.Models
{
    public class ClassSchedule
    {
        public int classScheduleId {  get; set; }
        public virtual ICollection<Session> sessions { get; set; }
        public int studentClassId { get; set; }
        public ClassSchedule() { }
        public ClassSchedule(int classId)
        {
            this.studentClassId = classId;
            sessions = new List<Session>();
        }
        public void AddSession(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session), "Sesja nie może być null.");
            }
            sessions.Add(session);
        }
        public void RemoveSession(Session session)
        {
            sessions.Remove(session);
        }
    }
}
