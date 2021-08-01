import * as axios from "../http.adapter"

import React, { useEffect, useState } from "react"

import { Image } from "./BirdImage"
import { Link } from "react-router-dom"
import { MapWrap } from "./MapWrap"
import moment from "moment"

export const PhotoPage: React.FC<any> = props => {
    const [image, setImage] = useState<Image | null>(null)
    const photoId = props.match.params.id

    useEffect(() => {
        axios.get(`/api/gallery/photo/${photoId}`).then((res) => {
            const receivedImage = res.data.photo
            receivedImage.tags = [{title: receivedImage.caption, value: receivedImage.caption}]
            setImage(receivedImage)
        })
    }, [props])

    if (!image)
        return null

    return (
            <div className="body">

            {image.birdIds.map(id => <Link
                    key={id}
                    className="big-link" to={`/birds/${id}`}>
                    {image.caption} ({moment(image.dateTaken).format("YYYY MM DD")})
                </Link>,
            )}
                <section className="photo-flex-container">
                    <div className="flex-item photo-flex-element">
                        <img src={image.original} className="photo-page"></img>
                    </div>
                    <div className="flex-item">
                        <MapWrap asPopup={true} photoId={photoId}/>
                    </div>
                </section>
            </div>)
}
