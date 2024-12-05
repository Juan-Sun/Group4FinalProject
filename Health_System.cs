using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Group4FinalProject
{
	internal class Health_System
	{

		//replace player with proper player property
		public bool didCollisionHappen(Player p, Enemies e)
		{
			/*int playerRadius = p.getRadius();
			int eRadius = Enemies.radius;
			int distance = playerRadius + eRadius;
			if (Vector2.Distance(p.position, e.position) < distance)
			{
				return true;
			}*/

			return false;
		}
	}
}