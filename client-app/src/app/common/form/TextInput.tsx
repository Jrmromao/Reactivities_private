import React from 'react'
import { FieldRenderProps } from 'react-final-form'
import { FormFieldProps, Form, Label } from 'semantic-ui-react'

interface IProps extends FieldRenderProps<string, HTMLElement>, FormFieldProps { }

const TextInput: React.FC<IProps> = ({
    input,
    width,
    type,
    placeholder,
    meta: { touched, error } }) => {
    return (
            <Form.Field error={error && !!error} width={width}>
                <input {...input} placeholder={placeholder} type={type}/>
                    {touched && error && (
                        <Label basic color='red'>{error}</Label>
                    )}
               
            </Form.Field>

    )
}

export default TextInput;
