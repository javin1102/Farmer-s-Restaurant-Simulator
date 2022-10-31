using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static bool HasReachedDestination( this NavMeshAgent agent, float offset = 0 ) => agent.remainingDistance - offset <= agent.stoppingDistance;
    public static Vector3 Round( this Vector3 vec )
    {
        vec.x = Mathf.Round( vec.x );
        vec.y = Mathf.Round( vec.y );
        vec.z = Mathf.Round( vec.z );
        return vec;
    }

    public static void SetLayer( this Transform trans, int layer )
    {
        if ( trans.gameObject.layer == Utils.UILayer ) return;
        trans.gameObject.layer = layer;
        foreach ( Transform child in trans )
            child.SetLayer( layer );
    }

    public static int LevenshteinDistance( string src, string dest )
    {
        int[,] d = new int[src.Length + 1, dest.Length + 1];
        int i, j, cost;
        char[] str1 = src.ToCharArray();
        char[] str2 = dest.ToCharArray();

        for ( i = 0; i <= str1.Length; i++ )
        {
            d[i, 0] = i;
        }
        for ( j = 0; j <= str2.Length; j++ )
        {
            d[0, j] = j;
        }
        for ( i = 1; i <= str1.Length; i++ )
        {
            for ( j = 1; j <= str2.Length; j++ )
            {

                if ( str1[i - 1] == str2[j - 1] )
                    cost = 0;
                else
                    cost = 1;

                d[i, j] =
                    Mathf.Min(
                        d[i - 1, j] + 1,              // Deletion
                        Mathf.Min(
                            d[i, j - 1] + 1,          // Insertion
                            d[i - 1, j - 1] + cost ) ); // Substitution

                if ( ( i > 1 ) && ( j > 1 ) && ( str1[i - 1] ==
                    str2[j - 2] ) && ( str1[i - 2] == str2[j - 1] ) )
                {
                    d[i, j] = Mathf.Min( d[i, j], d[i - 2, j - 2] + cost );
                }
            }
        }

        return d[str1.Length, str2.Length];
    }


    public static bool Search( string word, string dest )
    {
        int levenshteinDistance = LevenshteinDistance( word, dest );
        int length = Mathf.Max( word.Length, dest.Length );
        double score = 1.0f - ( double ) levenshteinDistance / length;
        //
        float fuzz = 1 - length / (length - word.Length);
        if ( score > fuzz ) return true;
        else return false;
    }
}
