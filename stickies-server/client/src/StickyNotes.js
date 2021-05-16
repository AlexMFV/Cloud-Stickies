import React from 'react'
import Sticky from './Sticky'

export default function StickyNotes({ stickies, clickSticky }) {
    return (
        stickies.map(sticky =>{
            return <Sticky key={ sticky.note_id } clickSticky={clickSticky} sticky={ sticky } />
        })
    )
}
