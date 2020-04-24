using System;

namespace ProveedorA
{
    class Program
    {
        static void Main(string[] args)
        {
            new ImplementacionProveedor().run().GetAwaiter().GetResult();
        }
    }
}
