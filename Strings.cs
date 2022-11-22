namespace apiTest;

public class Strings
{
    public unsafe static int CompareUnsafe(string s1, string s2)
    {
        (var ne1, var ne2) = (string.IsNullOrEmpty(s1), string.IsNullOrEmpty(s2));

        if (ne1 && ne2) return 0;
        if (ne1) return -1;
        if (ne2) return 1;

        fixed (char* pointer1 = s1, pointer2 = s2)
        {
            var p1 = pointer1;
            var p2 = pointer2;

            while (*p1 != 0)
            {
                if (*p2 == 0) return 1;

                if (*p1 >= '0' && *p1 <= '9' && *p2 >= '0' && *p2 <= '9')
                {
                    (var num1, var num2) = (*p1 - '0', *p2 - '0');
                    p1++; p2++;

                    while (*p1 >= '0' && *p1 <= '9')
                    {
                        num1 = 10 * num1 + *p1 - '0';
                        p1++;
                    }

                    while (*p2 >= '0' && *p2 <= '9')
                    {
                        num2 = 10 * num2 + *p2 - '0';
                        p2++;
                    }

                    if (num1 != num2) return num1 > num2 ? 1 : -1;
                }
                else
                {
                    if (*p1 != *p2) return (*p1 > *p2) ? 1 : -1;

                    p1++; p2++;
                }
            }

            return *p2 == 0 ? 0 : -1;
        }
    }
}
