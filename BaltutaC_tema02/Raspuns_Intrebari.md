# R?spunsuri

1. Ce este un viewport?
   Viewport-ul este zona dreptunghiular? din fereastr? unde OpenGL deseneaz? con?inutul grafic. Dimensiunile ?i pozi?ia acestuia sunt setate cu func?ia `GL.Viewport(x, y, width, height)`.

2. Ce reprezint? conceptul de FPS în OpenGL? 
   FPS (frames per second) reprezint? num?rul de cadre desenate într-o secund?. Acest concept indic? fluiditatea aplica?iei ?i performan?a sistemului de randare.

3. Când este rulat? metoda `OnUpdateFrame()`? 
   Metoda este apelat? periodic, înainte de fiecare randare, pentru a actualiza logica aplica?iei. Aici se gestioneaz? pozi?ia obiectelor sau alte modific?ri dinamice.

4. Ce este modul imediat de randare? 
   Modul imediat este o metod? veche ?i simpl? de randare, în care primitivile geometrice sunt specificate direct folosind apeluri de func?ii precum `GL.Begin()` ?i `GL.End()`. Este u?or de utilizat, dar ineficient.

5. Care este ultima versiune de OpenGL care accept? modul imediat?  
   Modul imediat este acceptat pân? la OpenGL 3.0. Din OpenGL 3.1 a fost eliminat în favoarea pipeline-ului programabil.

6. Când este rulat? metoda `OnRenderFrame()`?
   Metoda este apelat? de fiecare dat? când se randeaz? un nou cadru, con?inând instruc?iunile pentru desenarea scenei.

7. De ce este nevoie ca metoda `OnResize()` s? fie executat? cel pu?in o dat??
   Aceast? metod? seteaz? viewport-ul ?i matricea de proiec?ie conform dimensiunilor ferestrei. F?r? apelul acestei metode, scena poate fi distorsionat? sau afi?at? incorect.

8. Ce reprezint? parametrii metodei `CreatePerspectiveFieldOfView()` ?i care este domeniul de valori pentru ace?tia?
   Parametrii metodei:
   - FOV (field of view): Unghiul de deschidere al camerei, în radiani (de obicei între 0.5 ?i 2 radiani).
   - Aspect Ratio: Raportul l??ime/înal?ime al ferestrei.
   - Near Plane: Distan?a minim? de vizibilitate a obiectelor (pozitiv?).
   - Far Plane: Distan?a maxim? de vizibilitate a obiectelor (mai mare decât near plane).