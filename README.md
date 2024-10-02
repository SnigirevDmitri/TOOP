Задание: Разработать систему классов для решения задачи оптимизации на базе следующих интерфейсов:

1) IParametricFunction - параметрическая функция, Bind фиксирует параметры и возвращает следующие реализации:
   
   1. Линейная функция в n-мерном пространстве (число параметров n+1, реализует IDifferentiableFunction)
   2. Полином n-й степени в одномерном пространстве (число параметров n+1, не реализует IDifferentiableFunction)
   3. Кусочно-линейная функция (реализует IDifferentiableFunction)
   4. Сплайн (не линейный)

2) IFunctional - минимизируемый функционал, должны быть следующие реализации:
   
   1. l1 норма разности с требуемыми значениями в наборе точек (реализует IDifferentiableFunctional, не реализует ILeastSquaresFunctional)
   2. l2 норма разности с требуемыми значениями в наборе точек (реализует IDifferentiableFunctional, реализует ILeastSquaresFunctional)
   3. linf норма разности с требуемыми значениями в наборе точек (не реализует IDifferentiableFunctional, не реализует ILeastSquaresFunctional)
   4. Интеграл по некоторой области (численно)

3) IOptimizator - метод минимизации, должны быть следующие реализации:

   1. (универсальный) Метод Монте-карло (лучше алгоритм имитации отжига)
   2. (требующий IDifferentiableFunctional) Метод градиентного спуска (лучше метод сопряжённых градиентов)
   3. (требующий ILeastSquaresFunctional) Алгоритм Гаусса-Ньютона

 
Обязательные требования:

   1. Интерфейсы из задания менять нельзя
   2. Классы должны взаимодействовать только через эти интерфейсы
