// Skip list implementation by Shobhit Mishra

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkipList
{
    class SkipList
    {
        const int MAXLEVEL = 32;

        class Node
        {
            public int value;
            public int level;
            public Node[] next;

            public Node(int Value, int Level)
            {
                value = Value;
                level = Level;
                next = new Node[Level];
            }
        }

        // This is the current height of the skiplist
        public int skipListHeight = 1;
                
        // skip list head serves as the head node at every level
        Node skipListHead = new Node(-Int32.MaxValue, MAXLEVEL);

        Random flipcoin = new Random();

        public void Insert(int datum)
        {            
            int nodelevel = 1;

            // flip a coin to decide the level of the node. Everytime we get a head (1)
            // we promote the node to a higher level.
            while (flipcoin.Next(0, 2) == 1)
                nodelevel++;

            // the maximum level is 32. If the node is higher than 32 then set it back to 32.
            if (nodelevel > MAXLEVEL)
                nodelevel = MAXLEVEL;

            // adjust the height of skip list if the level of current node is more than the skip list height.
            if (skipListHeight < nodelevel)
                skipListHeight = nodelevel;

            //Debugging message.
#if DEBUG
            Console.WriteLine("value is {0} ans level is {1}", datum, nodelevel);
#endif

            // create the node
            Node newnode = new Node(datum, nodelevel);

            //We have to insert this node.
            Node currnode = skipListHead;

            // start at the top of the list. 
            // if the value of the next node is greater than the value that has to be 
            // inserted then break from inner loop which is equivalent to going one level down.            
            for (int i = skipListHeight -1; i >= 0; i--)
            {
                for (; currnode.next[i] !=null; currnode = currnode.next[i])
                {
                    if (currnode.next[i].value > datum)
                        break;
                }

                // If the current level is less than node level then insert the node. 
                if (i < nodelevel)
                {
                    // add this node in this level
                    newnode.next[i] = currnode.next[i];
                    currnode.next[i] = newnode;
                }
            }
        }

        public bool SearchNode(int datum)
        {
            Node currnode = this.skipListHead;

            for (int i = this.skipListHeight - 1; i >= 0; i--)
            {
                for (; currnode.next[i] != null; currnode = currnode.next[i])
                {
                    if (currnode.next[i].value == datum)
                        return true;

                    if (currnode.next[i].value > datum)
                        break;
                }
            }

            return false;   
        }

        public void PrintSkipList()
        {
            // Traverse all the levels
            for (int i = this.skipListHeight -1 ; i >= 0; i--)
            {
                Console.WriteLine("Nodes at level {0}", i + 1);

                // we don't want to print the value of skiplisthead. It is always -Int32.Max. 
                Node currnode = this.skipListHead.next[i];

                // print all the nodes at this level
                while (currnode !=null)
                {
                    Console.Write(currnode.value + "   ");
                    currnode = currnode.next[i];
                }
                Console.WriteLine();
            }
        }

        public void RemoveFromSkipList(int datum)
        {
            Node currentNode = this.skipListHead;            

            for (int i = this.skipListHeight -1; i >= 0; i--)
            {
                for (; currentNode.next[i] != null; currentNode = currentNode.next[i])
                {
                    if (currentNode.next[i].value == datum)
                    {
                        currentNode.next[i] = currentNode.next[i].next[i];

                        // Reduce the height if this was the top level head
                        if (currentNode == this.skipListHead && currentNode.next[i] == null)
                            this.skipListHeight--;
                       
                        break;
                    }

                    else if (currentNode.next[i].value > datum)
                        break;
                }
            }           
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SkipList sl = new SkipList();
            sl.Insert(10);
            sl.Insert(3);
            sl.Insert(21);
            sl.Insert(41);
            sl.Insert(6);
            sl.Insert(80);
            sl.Insert(63);
            sl.Insert(79);
            sl.Insert(23);
            sl.Insert(1);
            
            sl.PrintSkipList();
                        
            while (true)
            {
                Console.WriteLine("Enter a node to Remove. Enter -1 to exit");
                int num = Int32.Parse(Console.ReadLine());

                if (num == -1)
                    break;

                sl.RemoveFromSkipList(num);

                sl.PrintSkipList();

                //if(sl.SearchNode(num))
                //    Console.WriteLine("data found");
                //else
                //    Console.WriteLine("Data not found");
            }
        }
    }
}
