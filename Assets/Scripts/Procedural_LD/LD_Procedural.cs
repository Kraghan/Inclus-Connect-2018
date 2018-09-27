using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LD_Procedural : MonoBehaviour
{
	public int Nombre_Obstacles;
	public int Dist_obs_Facile, Dist_obs_Moyen, Dist_obs_Difficile;
	public int Dist_serie_Facile, Dist_serie_Moyen, Dist_serie_Difficile;
	public int Parcours_Facile, Parcours_Moyen, Parcours_Difficile;

	void Generate_LD ()
	{
		int Dist_Gener = 0;
		int nb_obst = 0;
		int dist_separ_serie = 0;
		int dist_separ_obstacles = 0;
		int dist_separ_difficulte = 0;

		while (nb_obst < Nombre_Obstacles) {
			if (Dist_Gener <= Parcours_Facile) 
			{
				dist_separ_serie = Dist_serie_Facile;
				dist_separ_obstacles = Dist_obs_Facile;
			} else if (Dist_Gener >= Parcours_Facile && Dist_Gener <= Parcours_Moyen) 
			{
				dist_separ_serie = Dist_serie_Moyen;
				dist_separ_obstacles = Dist_obs_Moyen;				
			} else if (Dist_Gener >= Parcours_Difficile) 
			{
				dist_separ_serie = Dist_serie_Difficile;
				dist_separ_obstacles = Dist_obs_Difficile;				
			}
				
			int nb_obs_serie = Random.Range (0, 4); //Je choisis combien la série aura d'obstacles (entre 1 et 4)

			for (int i = 0; i < nb_obs_serie; i++) {
				
			}
		}
	}

	void Start ()
	{
		Generate_LD ();
	}
}
