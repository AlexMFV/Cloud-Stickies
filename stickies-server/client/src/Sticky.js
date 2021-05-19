import React, {useEffect} from 'react'
import './stickies.css'

export default function Sticky({ sticky, clickSticky }) {

    useEffect(() => {
        document.getElementById(sticky.note_id).setAttribute("style", "background-color: #" + sticky.noteColor.substring(3, sticky.noteColor.length) + ";")
    })

    function handleStickyClick(){
        clickSticky(sticky.note_id);
    }

    return (
        <>
        <div id={sticky.note_id} className="note" onClick={handleStickyClick}>
            {sticky.noteTitle}
        </div>
        </>
    )
}