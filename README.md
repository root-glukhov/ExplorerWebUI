# ExplorerWebUI

Предполагаю, что каталоги должны подгружаться асинхронно, так как подсчет размера папок трудоемкий и при переходе между ними запрос может долго выполняться. 
Я пытался это реализовать через частичные представления и ajax-запросы. Проблема в том, что частичное представление не может обратиться к js родителя (странице Index).
По этому возможно спуститься от корня только на один уровень. То что получилось из этого закоммитил в отдельную ветку - Ajax.
